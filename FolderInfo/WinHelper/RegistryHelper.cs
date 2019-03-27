using Microsoft.Win32;

namespace FolderInfo.WinHelper
{
    class RegistryHelper
    {
        /// <summary>
        /// Returns windows register value
        /// </summary>
        /// <param name="key">Register key</param>
        /// <param name="subkeyName">Register subkey</param>
        /// <param name="valueName">Target value name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static object GetRegistryValue(RegistryKey key,string subkeyName, string valueName, object defaultValue)
        {
            RegistryKey subkey = key.OpenSubKey(subkeyName, false);
            object result = subkey.GetValue(valueName, defaultValue);
            subkey.Close();
            return result;
        }
    }
}
