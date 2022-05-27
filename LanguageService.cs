using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using PRM.Model;
using Shawn.Utils.Interface;
using Shawn.Utils.Wpf;

namespace PRM.Service
{
    public class Lang
    {
        public string key { get; set; }
        public string enus { get; set; }
        public string zhcn { get; set; }
        public string dede { get; set; }
        public string frfr { get; set; }
        public string ptbr { get; set; }

        private static string GetString(string value, string @default = "")
        {
            if (value == @default)
                return "";
            return value.IndexOf(",") >= 0 ? $"\"{value}\"" : value;
        }

        public static string Title()
        {
            return "key,en-us,zh-cn-google,zh-cn,de-de-google,de-de,fr-fr-google,fr-fr,pt-br-google,pt-br";
        }
        public override string ToString()
        {
            var ret = GetString(key);
            ret += "," + GetString(enus);
            ret += ",," + GetString(zhcn, enus);
            ret += ",," + GetString(dede, enus);
            ret += ",," + GetString(frfr, enus);
            ret += ",," + GetString(ptbr, enus);
            return ret;
        }
    }
    public class LanguageService : ILanguageService
    {
        private string _languageCode = "en-us";
        private readonly ResourceDictionary _applicationResourceDictionary;


        public readonly Dictionary<string, ResourceDictionary> Resources = new Dictionary<string, ResourceDictionary>();

        /// <summary>
        /// code => language file name, all codes leave in small cases, ref https://en.wikipedia.org/wiki/Language_code
        /// </summary>
        public Dictionary<string, string> LanguageCode2Name { get; } = new Dictionary<string, string>();


        public LanguageService(ResourceDictionary applicationResourceDictionary)
        {
            _applicationResourceDictionary = applicationResourceDictionary;
            // add static language resources
            AddStaticLanguageResources("en-us");
            AddStaticLanguageResources("zh-cn");
            AddStaticLanguageResources("de-de");
            AddStaticLanguageResources("fr-fr");
            AddStaticLanguageResources("pt-br");



#if DEBUG
            
            using (var writer = new StreamWriter("lang_prm.csv", Encoding.UTF8, new FileStreamOptions()
            {
                Access = FileAccess.ReadWrite,
                Mode = FileMode.Create
            }))
            //using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InstalledUICulture)
            //{
            //}))
            {
                var items = new List<Lang>();


                var keys = new List<string>()
                {"language_name",
"messagebox_title_warning",
"messagebox_title_error",
"messagebox_title_info",
"OK",
"Exit",
"Close",
"Add",
"Edit",
"Save",
"Save as...",
"Back",
"Delete",
"Cancel",
"Select",
"Explore to...",
"Previous",
"Next",
"Done",
"Reset",
"Import",
"Export",
"Name",
"File Name",
"Test",
"Reconnect",
"Dismiss",
"Lock",
"Always on top",
"Close all",
"Rename",
"Connect",
"Connect (New window)",
"Note",
"Share",
"Copy",
"filter_mainwindow",
"Search (Ctrl+F)",
"hotkey_already_registered",
"hotkey_registered_fail",
"string_permission_denied",
"All",
"Protocol",
"file_transmit_host_list_header_last_update",
"file_transmit_host_list_header_type",
"file_transmit_host_list_header_size",
"file_transmit_host_message_preview_over_size",
"file_transmit_host_message_files_download_to",
"file_transmit_host_message_files_download_to_dir",
"file_transmit_host_message_select_files_to_upload",
"file_transmit_host_command_refresh",
"file_transmit_host_command_create_folder",
"file_transmit_host_command_upload",
"file_transmit_host_command_select_files_upload",
"file_transmit_host_command_select_folder_upload",
"file_transmit_host_command_delete",
"file_transmit_host_command_save_to",
"file_transmit_host_warning_same_names_title",
"file_transmit_host_warning_same_names",
"host_reconecting_info",
"Options",
"About",
"Toggle Cards/List",
"system_options_general_title",
"Language",
"Startup",
"system_options_general_start_when_win_login",
"system_options_general_start_minimized",
"Confirm before closing",
"system_options_general_log_file_path",
"system_options_quick_description",
"system_options_quick_match_providers",
"Launcher",
"system_options_quick_launcher_enable",
"system_options_quick_launcher_hotkey",
"system_options_data_security_title",
"system_options_data_security_database",
"system_options_data_security_database_migrate",
"system_options_data_security_rsa_title",
"system_options_data_security_rsa_encrypt",
"system_options_data_security_rsa_encrypt_dialog_title",
"system_options_data_security_rsa_decrypt",
"system_options_data_security_rsa_public_key",
"system_options_data_security_rsa_private_key_path",
"system_options_data_security_export_dialog_title",
"system_options_data_security_info_data_processing",
"system_options_data_security_info_encrypt_private_key_warning",
"system_options_data_security_info_clear_rsa",
"system_options_data_security_error_can_not_open",
"string_database_error_permission_denied",
"string_database_error_rsa_private_key_format_error",
"string_database_error_rsa_private_key_not_found",
"string_database_error_rsa_private_key_not_match",
"Theme",
"Color",
"Color1",
"Color2",
"Background",
"Normal",
"Lighter",
"Darker",
"Font",
"Font size",
"Themes",
"server_card_operate_duplicate",
"server_card_operate_copy_address",
"server_card_operate_copy_username",
"server_card_operate_copy_password",
"confirm_to_delete",
"confirm_to_delete_selected",
"import_server_dialog_title",
"import_from_json",
"import_from_csv",
"import_done_0_items_added",
"import_failure_with_data_format_error",
"server_editor_different_options",
"server_editor_bulk_editing",
"server_editor_bulk_editing_title",
"server_editor_multi_selected_count",
"server_editor_group_title_common",
"Connection",
"server_editor_group_title_display",
"server_editor_group_title_advantage",
"server_editor_group_title_gateway",
"Tags",
"server_editor_tags_tag",
"Logo",
"server_editor_cmd_before_connected",
"server_editor_cmd_before_connected_tag",
"server_editor_cmd_after_disconnected",
"server_editor_cmd_after_disconnected_tag",
"Hostname",
"Target",
"Port",
"User",
"Password",
"Use private key",
"Private key",
"Open SFTP when connected",
"server_editor_display_full_screen_flag",
"server_editor_display_full_screen_flag_window",
"server_editor_display_full_screen_flag_full_screen",
"server_editor_display_full_screen_flag_all_screens",
"Display the connection bar when use the full screen",
"New session always use full screen",
"New session use full screen if the last session is full screen",
"Resolution",
"Fit to window",
"Custom resolution (stretch)",
"Custom resolution (fixed)",
"Full-screen resolution (stretch)",
"Full-screen resolution (fixed)",
"server_editor_display_rdp_performance",
"server_editor_display_rdp_performance_auto",
"server_editor_display_rdp_performance_high",
"server_editor_display_rdp_performance_middle",
"server_editor_display_rdp_performance_low",
"server_editor_advantage_admin",
"server_editor_advantage_resources",
"server_editor_advantage_clipboard",
"server_editor_advantage_key_combinations",
"server_editor_advantage_disk_drives",
"server_editor_advantage_sounds",
"server_editor_advantage_sounds_disabled",
"server_editor_advantage_sounds_on_local",
"server_editor_advantage_sounds_on_remote",
"server_editor_advantage_audio_capture",
"server_editor_advantage_ports",
"server_editor_advantage_printers",
"server_editor_advantage_smart_cards",
"server_editor_advantage_ssh_version",
"server_editor_advantage_ssh_startup_auto_command",
"server_editor_advantage_ssh_startup_auto_kitty_session_tip",
"server_editor_advantage_sftp_startup_path",
"server_editor_gateway_mode",
"server_editor_gateway_mode_automatically_detect",
"server_editor_gateway_mode_use_these",
"server_editor_gateway_mode_do_not_use",
"server_editor_gateway_server_host_name",
"server_editor_gateway_logon_method",
"server_editor_gateway_logon_method_psw",
"server_editor_gateway_logon_method_smart_card",
"server_editor_gateway_bypass_gateway_for_local_address",
"server_editor_remote_app_name",
"server_editor_remote_app_name_tag",
"server_editor_remote_app_fullname",
"server_editor_remote_app_fullname_tag",
"about_page_how_to_use",
"about_page_feedback",
"guidance_introduce",
"guidance_feature_launcher_key",
"guidance_feature_launcher_select",
"guidance_feature_launcher_connect",
"guidance_feature_launcher_menu",
"guidance_feature_tab_demo",
"guidance_feature_start_setting1",
"guidance_feature_start_setting2",
"word_oops",
"error_report_unexpected_error_occurred",
"error_report_to_clipboard",
"error_report_please_help_us_improve",
"error_report_how_help_us_improve",
"error_report_send_by_email",
"error_report_send_by_github",
"logo_selector_open_file_dialog_title",
"Default",
"Exe path",
"Cmd parameter",
"New runner",
"New runner name",
"Can not be empty!",
"Hosting",
"Hosting this exe in PRemoteM tab view?",
"Caution: some exe can not be hosted in PRemoteM.",
"Show environment variables",
"How to select the specified monitors",
"mstsc.exe mode",
"Enabled",
"In this mode RDP will start the session in the way mstsc.exe xxxx.rdp in CMD, it can offer more custom parameters.",
"Note: This mode will be automatically enabled if there are 2+ monitors with different display scaling on your PC and you selected the full screen mode.",
"Additional settings",
"Helper",
"Preview",
"Preview rdp file",
"Check monitor number",
"Environment variables",
"Add environment variables",
"Selected",
"Custom",
"write permissions alert",
"Follow system",
"Scale",
"tag_tooltips",
"Are you sure you want to close the connection?",
"Hi there,",
"I hope that you find this app useful. If you would like to support my work, you can buy me a coffee or give a nice review. Thanks!",
"Give suggestions",
"Buy a coffee",
"Give nice review",
"Do not show this again in current version",
"User profile",
"Portable mode",
"Profile is saved where executable is located",
"Install for current windows account",
"Profile is saved in your Windows AppData folder",
                };

                foreach (var key in keys)
                {
                    var lang = new Lang()
                    {
                        key = key,
                        enus = Resources["en-us"][key].ToString()?.Replace("\r\n", "&#13;&#10;") ?? key,
                        zhcn = Resources["zh-cn"][key].ToString()?.Replace("\r\n", "&#13;&#10;") ?? key,
                        dede = Resources["de-de"][key].ToString()?.Replace("\r\n", "&#13;&#10;") ?? key,
                        frfr = Resources["fr-fr"][key].ToString()?.Replace("\r\n", "&#13;&#10;") ?? key,
                        ptbr = Resources["pt-br"][key].ToString()?.Replace("\r\n", "&#13;&#10;") ?? key,
                    };
                    items.Add(lang);
                }

                //csvWriter.WriteRecords(items);
                writer.WriteLine(Lang.Title());
                foreach (var item in items)
                {
                    writer.WriteLine(item.ToString());
                    //csvWriter.WriteRecord(item);
                }
            }
#endif
        }

        public void AddXamlLanguageResources(string code, string fullName)
        {
            var resourceDictionary = GetResourceDictionaryByXamlFilePath(fullName);
            if (resourceDictionary?.Contains("language_name") != true) return;
            AddLanguage(code, resourceDictionary["language_name"].ToString()!, resourceDictionary);
        }

        private void AddStaticLanguageResources(string code)
        {
            //var path = $"pack://application:,,,/PRM.Core;component/Languages/{code}.xaml";
            var path = ResourceUriHelper.GetUriPathFromCurrentAssembly($"Resources/Languages/{code}.xaml");
            if (LanguageCode2Name.ContainsKey(code)) return;
            var r = GetResourceDictionaryByXamlUri(path);
            Debug.Assert(r != null);
            Debug.Assert(r.Contains("language_name"));
            AddLanguage(code, r["language_name"].ToString()!, r);
        }

        private static ResourceDictionary? GetResourceDictionaryByXamlUri(string path)
        {
            try
            {
                var resourceDictionary = MultiLanguageHelper.LangDictFromXamlUri(new Uri(path));
                if (resourceDictionary != null)
                {
                    return resourceDictionary;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        private static ResourceDictionary? GetResourceDictionaryByXamlFilePath(string path)
        {
            Debug.Assert(path.EndsWith(".xaml", true, CultureInfo.InstalledUICulture));
            try
            {
                var resourceDictionary = MultiLanguageHelper.LangDictFromXamlFile(path);
                if (resourceDictionary != null)
                {
                    return resourceDictionary;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


        private void AddLanguage(string code, string name, ResourceDictionary resourceDictionary)
        {
            if (LanguageCode2Name.ContainsKey(code))
            {
                LanguageCode2Name[code] = name;
                Resources[code] = resourceDictionary;
            }
            else
            {
                LanguageCode2Name.Add(code, name);
                Resources.Add(code, resourceDictionary);
            }
        }

        public bool SetLanguage(string code)
        {
            if (!LanguageCode2Name.ContainsKey(code))
                return false;

            _languageCode = code;
            var resource = Resources[code];

            var en = Resources["en-us"];
            var missingFields = MultiLanguageHelper.FindMissingFields(en, resource);
            if (missingFields.Count > 0)
            {
                foreach (var field in missingFields)
                {
                    resource.Add(field, en[field]);
                }
#if DEBUG
                var mf = string.Join(", ", missingFields);
                MessageBox.Show($"language resource missing:\r\n {mf}", Translate("messagebox_title_error"), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                File.WriteAllText("LANGUAGE_ERROR.txt", mf);
#endif
            }

            _applicationResourceDictionary?.ChangeLanguage(resource);
            GlobalEventHelper.OnLanguageChanged?.Invoke();
            return true;
        }

        public string Translate(Enum e)
        {
            var key = e.GetType().Name + e;
            return Translate(key);
        }

        public string Translate(string key)
        {
            key = key.Trim(new[] { '\'' });
            if (_applicationResourceDictionary.Contains(key))
                return _applicationResourceDictionary[key].ToString() ?? key;

#if DEBUG
            var tw = new StreamWriter("need translation " + _languageCode + ".txt", true);
            tw.WriteLine(key);
            tw.Close();
#endif

            return key;
        }

        public string Translate(string key, params object[] parameters)
        {
            var format = Translate(key);
            if (format == null)
                return "!" + key + (parameters.Length > 0 ? ":" + string.Join(",", parameters) : "") + "!";

            return string.Format(format, parameters);
        }
    }
}
