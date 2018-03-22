using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace WorldEditor.Config
{
    public class PathExistsRule : ValidationRule
    {
        public bool IsDirectory { get; set; }

        public string BasePath { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string))
            {
                return new ValidationResult(false, "Path must be a string");
            }

            string path = (!string.IsNullOrEmpty(BasePath)) ? Path.Combine(BasePath, value.ToString()) : value.ToString();
            if (IsDirectory && !Directory.Exists(path))
            {
                return new ValidationResult(false, "Directory not found");
            }

            if (!IsDirectory && !File.Exists(path))
            {
                return new ValidationResult(false, "File not found");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
