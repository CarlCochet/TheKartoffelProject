using System;
using System.Reflection;
using System.Text;
using System.Xml;
using WorldEditor.Helpers.Reflection;

namespace WorldEditor.Config.Configuration.Config
{
    public class XmlConfigNode
    {
        private object m_newValue;
        public XmlNode Node
        {
            get;
            private set;
        }
        public string Namespace
        {
            get;
            private set;
        }
        public string AssemblyName
        {
            get;
            private set;
        }
        public string ClassName
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public bool Serialized
        {
            get;
            set;
        }
        public string Path
        {
            get
            {
                return string.Concat(new string[]
				{
					this.Namespace,
					".",
					this.ClassName,
					".",
					this.Name
				});
            }
        }
        public string Documentation
        {
            get;
            set;
        }
        public VariableAttribute Attribute
        {
            get;
            private set;
        }
        public FieldInfo BindedField
        {
            get;
            private set;
        }
        public PropertyInfo BindedProperty
        {
            get;
            private set;
        }
        public object Instance
        {
            get;
            set;
        }

        public XmlConfigNode(XmlNode node)
        {
            this.Node = node;
            this.Name = ((node.Attributes["name"] != null) ? node.Attributes["name"].Value : "");
            this.Serialized = (node.Attributes["serialized"] != null && node.Attributes["serialized"].Value == "true");
            this.ClassName = XmlConfigNode.GetClassNameFromNode(node);
            this.Namespace = XmlConfigNode.GetNamespaceFromNode(node);
            this.Documentation = XmlConfigNode.FindDescription(node);
            this.Instance = null;
        }

        public XmlConfigNode(FieldInfo field)
        {
            this.BindToField(field);
            this.Name = field.Name;
            this.Serialized = (!field.FieldType.HasInterface(typeof(IConvertible)) || field.FieldType.IsEnum);
            this.ClassName = field.DeclaringType.Name;
            this.Namespace = field.DeclaringType.Namespace;
            this.Instance = null;
        }

        public XmlConfigNode(PropertyInfo property)
        {
            this.BindToProperty(property);
            this.Name = property.Name;
            this.Serialized = (!property.PropertyType.HasInterface(typeof(IConvertible)) || property.PropertyType.IsEnum);
            this.ClassName = property.DeclaringType.Name;
            this.Namespace = property.DeclaringType.Namespace;
            this.Instance = null;
        }

        public void BindToField(FieldInfo fieldInfo)
        {
            if (this.BindedProperty != null)
            {
                throw new Exception(string.Format("Node already binded to a property : {0}", this.BindedProperty.Name));
            }
            if (!fieldInfo.IsStatic)
            {
                throw new Exception(string.Format("A variable field have to be static : {0} is not static", fieldInfo.Name));
            }
            this.Attribute = fieldInfo.GetCustomAttribute<VariableAttribute>();
            if (this.Attribute == null)
            {
                throw new Exception(string.Format("{0} has no variable attribute", fieldInfo.Name));
            }
            this.BindedField = fieldInfo;
        }

        public void BindToProperty(PropertyInfo propertyInfo)
        {
            if (this.BindedField != null)
            {
                throw new Exception(string.Format("Node already binded to a field : {0}", this.BindedField.Name));
            }
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite)
            {
                throw new Exception(string.Format("{0} has not get and set accessors", this.BindedProperty.Name));
            }
            this.Attribute = propertyInfo.GetCustomAttribute<VariableAttribute>();
            if (this.Attribute == null)
            {
                throw new Exception(string.Format("{0} has no variable attribute", this.BindedProperty.Name));
            }
            this.BindedProperty = propertyInfo;
        }

        public object GetValue(bool realValue = false)
        {
            object result;
            if (this.BindedField != null && this.BindedProperty == null)
            {
                if (!realValue && this.m_newValue != null && !this.Attribute.DefinableRunning)
                {
                    result = this.m_newValue;
                }
                else
                {
                    result = this.BindedField.GetValue(this.Instance);
                }
            }
            else
            {
                if (!(this.BindedProperty != null) || !(this.BindedField == null))
                {
                    throw new Exception(string.Format("Cannot read the config node '{0}' because no member has been binded to it", this.Path));
                }
                if (!realValue && this.m_newValue != null && !this.Attribute.DefinableRunning)
                {
                    result = this.m_newValue;
                }
                else
                {
                    result = this.BindedProperty.GetValue(this.Instance, new object[0]);
                }
            }
            return result;
        }

        public void SetValue(object value, bool alreadyRunning = false)
        {
            if (this.BindedField != null && this.BindedProperty == null)
            {
                if (this.m_newValue == null && !alreadyRunning)
                {
                    this.BindedField.SetValue(this.Instance, value);
                }
                else
                {
                    if (this.Attribute.DefinableRunning)
                    {
                        this.BindedField.SetValue(this.Instance, value);
                    }
                }
                this.m_newValue = value;
            }
            else
            {
                if (this.BindedProperty != null && this.BindedField == null)
                {
                    if (this.m_newValue == null && !alreadyRunning)
                    {
                        this.BindedProperty.SetValue(this.Instance, value, new object[0]);
                    }
                    else
                    {
                        if (this.Attribute.DefinableRunning)
                        {
                            this.BindedProperty.SetValue(this.Instance, value, new object[0]);
                        }
                    }
                    this.m_newValue = value;
                }
            }
        }

        private static string GetNamespaceFromNode(XmlNode node)
        {
            StringBuilder stringBuilder = new StringBuilder();
            XmlNode parentNode = node.ParentNode;
            while (parentNode.ParentNode != null && parentNode.ParentNode != parentNode.OwnerDocument.DocumentElement)
            {
                stringBuilder.Insert(0, parentNode.ParentNode.Name + ".");
                parentNode = parentNode.ParentNode;
            }
            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
        }

        private static string GetClassNameFromNode(XmlNode node)
        {
            return node.ParentNode.Name;
        }

        private static string FindDescription(XmlNode node)
        {
            XmlNode previousSibling = node.PreviousSibling;
            string result;
            while (previousSibling != null && previousSibling.NodeType == XmlNodeType.Comment && previousSibling is XmlComment)
            {
                if (!(previousSibling as XmlComment).Value.StartsWith("Editable as Running : "))
                {
                    result = (previousSibling as XmlComment).Value;
                    return result;
                }
                previousSibling = previousSibling.PreviousSibling;
            }
            result = string.Empty;
            return result;
        }
    }
}