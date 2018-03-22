using System;
namespace WorldEditor.Editors.Files.D2I
{
    public sealed class D2ITextRow : D2IGridRow
    {
        private int m_id;
        private string m_text;
        public int Id
        {
            get
            {
                return m_id;
            }
            set
            {
                if (m_id == value)
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
        public D2ITextRow()
        {
        }
        public D2ITextRow(int id, string text)
        {
            m_id = id;
            m_text = text;
        }
        public override string GetKey()
        {
            return Id.ToString();
        }
    }
}
