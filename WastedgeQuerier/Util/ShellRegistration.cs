using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace WastedgeQuerier.Util
{
    public static class ShellRegistration
    {
        public static void Register(string registryClass, ShellRegistrationLocation location, ShellRegistrationConfiguration configuration)
        {
            if (registryClass == null)
                throw new ArgumentNullException(nameof(registryClass));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            bool modified = false;

            using (var baseKey =
                location == ShellRegistrationLocation.CurrentUser
                ? OpenKey(Registry.CurrentUser, "Software\\Classes", ref modified)
                : Registry.ClassesRoot)
            {
                if (configuration.FileExtension != null)
                {
                    using (var key = OpenKey(baseKey, configuration.FileExtension, ref modified))
                    {
                        string registeredClass = (string)key.GetValue(null);

                        if (registeredClass == null)
                        {
                            SetValue(key, null, registryClass, ref modified);

                            if (configuration.ContentType != null)
                                SetValue(key, "Content Type", configuration.ContentType, ref modified);
                            if (configuration.PerceivedType != null)
                                SetValue(key, "PerceivedType", configuration.PerceivedType, ref modified);
                        }
                        else if (registeredClass != registryClass)
                        {
                            using (var openWithKey = OpenKey(key, "OpenWithProgids", ref modified))
                            {
                                SetValue(openWithKey, registryClass, "", ref modified);
                            }
                        }
                    }
                }

                if (configuration.FriendlyAppName != null && configuration.ExecutableFileName != null)
                {
                    using (var key = OpenKey(baseKey, "Applications\\" + Path.GetFileName(configuration.ExecutableFileName), ref modified))
                    {
                        SetValue(key, "FriendlyAppName", configuration.FriendlyAppName, ref modified);
                    }
                }

                using (var key = OpenKey(baseKey, registryClass, ref modified))
                {
                    if (configuration.FriendlyTypeName != null)
                    {
                        SetValue(key, null, configuration.FriendlyTypeName, ref modified);
                        SetValue(key, "FriendlyTypeName", configuration.FriendlyTypeName, ref modified);
                    }

                    string defaultIconPath = configuration.DefaultIcon;

                    if (
                        defaultIconPath == null &&
                        configuration.DefaultIconIndex.HasValue &&
                        configuration.ExecutableFileName != null
                    )
                        defaultIconPath = configuration.ExecutableFileName + "," + configuration.DefaultIconIndex;

                    if (defaultIconPath != null)
                    {
                        using (var iconKey = OpenKey(key, "DefaultIcon", ref modified))
                        {
                            SetValue(iconKey, null, defaultIconPath, ref modified);
                        }
                    }

                    if (
                        configuration.OpenCommandArguments != null &&
                        configuration.ExecutableFileName != null
                    )
                    {
                        using (var commandKey = OpenKey(key, "Shell\\Open\\Command", ref modified))
                        {
                            SetValue(
                                commandKey,
                                null,
                                ShellEncode(configuration.ExecutableFileName) + " " + configuration.OpenCommandArguments,
                                ref modified
                            );
                        }
                    }
                }
            }

            if (modified)
                ShellUtil.NotifyFileAssociationsChanged();
        }

        private static RegistryKey OpenKey(RegistryKey baseKey, string subKey, ref bool modified)
        {
            string[] subKeyParts = subKey.Split(new[] { Path.DirectorySeparatorChar }, 2);

            var key = baseKey.OpenSubKey(subKeyParts[0], true);

            if (key == null)
            {
                modified = true;
                key = baseKey.CreateSubKey(subKeyParts[0]);
            }

            if (subKeyParts.Length == 1)
                return key;

            using (key)
            {
                return OpenKey(key, subKeyParts[1], ref modified);
            }
        }

        private static void SetValue(RegistryKey key, string name, string value, ref bool modified)
        {
            string currentValue = (string)key.GetValue(name);

            if (value != currentValue)
            {
                key.SetValue(name, value);
                modified = true;
            }
        }

        private static string ShellEncode(string arg)
        {
            if (String.IsNullOrEmpty(arg))
                return "";

            return "\"" + arg.Replace("\"", "\"\"") + "\"";
        }
    }
}
