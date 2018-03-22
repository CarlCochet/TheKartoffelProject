using System;
namespace WorldEditor.Editors.Files.D2I
{
    public sealed class D2ITextUiRow : D2IGridRow
    {
        private string m_id;
        private string m_text;
        public string Id
        {
            get
            {
                return m_id;
            }
            set
            {
                if (string.Equals(m_id, value, StringComparison.Ordinal))
                {
                    return;
                }
                m_id = value;
                OnPropertyChanged("Id");
            }
        }
        public override string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                if (string.Equals(m_text, value, StringComparison.Ordinal))
                {
                    return;
                }
                m_text = value;
                OnPropertyChanged("Text");
            }
        }
        public D2ITextUiRow()
        {
        }
        public D2ITextUiRow(string id, string text)
        {
            m_id = id;
            m_text = text;
        }
        public override string GetKey()
        {
            return Id;
        }
    }
}
