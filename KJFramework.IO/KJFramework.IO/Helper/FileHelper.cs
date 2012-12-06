using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KJFramework.Basic.Struct;
using KJFramework.Core.Native;
using KJFramework.IO.Exception;
using KJFramework.Logger;
using Microsoft.Win32;
using FileNotFoundException = System.IO.FileNotFoundException;

namespace KJFramework.IO.Helper
{
#if !MONO
    /// <summary>
    ///     文件帮助器, 提供了相关的基本操作
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        ///     打开指定文件, 并返回文件流 
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         文件全路径
        ///     </para>
        /// </param>
        /// <param name="mode" type="System.IO.FileMode">
        ///     <para>
        ///         打开模式
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回打开后的文件流
        /// </returns>
        public static FileStream OpenFile(String filePath, FileMode mode)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            return new FileStream(filePath, mode);
        }

        /// <summary>
        ///     创建文件, 并返回创建后的文件流
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         文件全路径
        ///     </para>
        /// </param>
        /// <param name="appendFile" type="bool">
        ///     <para>
        ///         是否覆盖文件的标示
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回创建后的文件流
        /// </returns>
        public static FileStream CreateFile(String filePath, bool appendFile)
        {
            if (File.Exists(filePath))
            {
                if (!appendFile)
                {
                    throw new FileHasExistsException();
                }
            }
            //创建文件
            return new FileStream(filePath, FileMode.Create);
        }

        /// <summary>
        ///     获取当前PC所有硬盘驱动器列表
        /// </summary>
        /// <returns>
        ///     返回硬盘驱动器列表
        /// </returns>
        public static DriveInfo[] GetDriveInfo()
        {
            return DriveInfo.GetDrives();
        }

        /// <summary>
        ///     获取路径的下一层目录列表
        /// </summary>
        /// <param name="path" type="string">
        ///     <para>
        ///         路径
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回目录列表
        /// </returns>
        public static String[] GetDirectory(String path)
        {
            return Directory.GetDirectories(path);
        }

        /// <summary>
        ///     获取路径的下一层文件列表
        /// </summary>
        /// <param name="path" type="string">
        ///     <para>
        ///         路径
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回文件列表
        /// </returns>
        public static String[] GetFiles(String path)
        {
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// 依据文件名读取图标，若指定文件不存在，则返回空值。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Icon GetIconByFileName(string fileName)
        {
            if (fileName == null || fileName.Equals(string.Empty)) return null;
            if (!File.Exists(fileName)) return null;

            Native.SHFILEINFO shinfo = new Native.SHFILEINFO();
            //Use this to get the small Icon
            Native.Win32API.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Native.Win32API.SHGFI_ICON | Native.Win32API.SHGFI_SMALLICON);
            //The icon is returned in the hIcon member of the shinfo struct
            Icon myIcon = Icon.FromHandle(shinfo.hIcon);
            return myIcon;
        }

        /// <summary>
        /// 给出文件扩展名（.*），返回相应图标
        /// 若不以"."开头则返回文件夹的图标。
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="isLarge"></param>
        /// <returns></returns>
        public static Icon GetIconByFileType(string fileType, bool isLarge)
        {
            if (fileType == null || fileType.Equals(string.Empty)) return null;

            RegistryKey regVersion;
            string regFileType;
            string regIconString = null;
            string systemDirectory = Environment.SystemDirectory + "\\";

            if (fileType[0] == '.')
            {
                //读系统注册表中文件类型信息
                regVersion = Registry.ClassesRoot.OpenSubKey(fileType, true);
                if (regVersion != null)
                {
                    regFileType = regVersion.GetValue("") as string;
                    regVersion.Close();
                    regVersion = Registry.ClassesRoot.OpenSubKey(regFileType + @"\DefaultIcon", true);
                    if (regVersion != null)
                    {
                        regIconString = regVersion.GetValue("") as string;
                        regVersion.Close();
                    }
                }
                if (regIconString == null)
                {
                    //没有读取到文件类型注册信息，指定为未知文件类型的图标
                    regIconString = systemDirectory + "shell32.dll,0";
                }
            }
            else
            {
                //直接指定为文件夹图标
                regIconString = systemDirectory + "shell32.dll,3";
            }
            string[] fileIcon = regIconString.Split(new char[] { ',' });
            if (fileIcon.Length != 2)
            {
                //系统注册表中注册的标图不能直接提取，则返回可执行文件的通用图标
                fileIcon = new string[] { systemDirectory + "shell32.dll", "2" };
            }
            Icon resultIcon = null;
            try
            {
                //调用API方法读取图标
                int[] phiconLarge = new int[1];
                int[] phiconSmall = new int[1];
                Native.Win32API.ExtractIconEx(fileIcon[0], Int32.Parse(fileIcon[1]), phiconLarge, phiconSmall, 1);
                IntPtr iconHnd = new IntPtr(isLarge ? phiconLarge[0] : phiconSmall[0]);
                resultIcon = Icon.FromHandle(iconHnd);
            }
            catch { }
            return resultIcon;

        }

        /// <summary>
        ///  删除指定路径文件
        /// </summary>
        /// <param name="fileName" type="string">
        ///     <para>
        ///         要删除的文件全路径
        ///     </para>
        /// </param>
        public static void Delete(String fileName)
        {
            File.Delete(fileName);
        }

        /// <summary>
        ///     提取制定文件全路径中的目录信息
        /// </summary>
        /// <param name="path" type="string">
        ///     <para>
        ///         要提取的文件全路径
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回指定路径字符串的目录信息
        /// </returns>
        public static String GetFileDir(String path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        ///     提供打开文件对话框 , 并设置文件过滤器，提取用户要打开文件的信息
        /// </summary>
        /// <param name="title" type="string">
        ///     <para>
        ///         打开文件对话框标题
        ///     </para>
        /// </param>
        /// <param name="typeFilter" type="string">
        ///     <para>
        ///         打开文件对话文件类别过滤器
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该用户选择的要打开文件的字符串
        /// </returns>
        public static String GetFileToOpen(String title, String typeFilter)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = title;
                dialog.Filter = typeFilter;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return null;
            }
        }

        /// <summary>
        ///     提供打开文件对话框，提取用户要打开文件的信息
        /// </summary>
        /// <param name="title" type="string">
        ///     <para>
        ///         打开文件对话框标题
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该用户选择的要打开文件的字符串
        /// </returns>
        public static String GetFileToOpen(String title)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = title;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return null;
            }
        }

        /// <summary>
        ///     提供保存文件对话框 , 并设置文件过滤器，提取用户要保存文件的信息
        /// </summary>
        /// <param name="title" type="string">
        ///     <para>
        ///         保存文件对话框标题
        ///     </para>
        /// </param>
        /// <param name="typeFilter" type="string">
        ///     <para>
        ///         保存文件对话文件类别过滤器
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该用户选择的要保存文件的字符串
        /// </returns>
        public static String GetFileToSave(String title, String typeFilter)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = title;
                dialog.Filter = typeFilter;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return null;
            }
        }

        /// <summary>
        ///     提供保存文件对话框，提取用户要保存文件的信息
        /// </summary>
        /// <param name="title" type="string">
        ///     <para>
        ///         保存文件对话框标题
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该用户选择的要保存文件的字符串
        /// </returns>
        public static String GetFileToSave(String title)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = title;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return null;
            }
        }

        /// <summary>
        ///     获取指定目录下所有文件名称
        /// </summary>
        /// <param name="directoryPath" type="string">
        ///     <para>
        ///         指定目录
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该指定目录下的所有文件名称字符串
        /// </returns>
        public static String[] GetDirectoryFiles(String directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        ///     获取指定目录下所有子目录名称
        /// </summary>
        /// <param name="directoryPath" type="string">
        ///     <para>
        ///         指定目录
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该指定目录下的所有子目录名称字符串
        /// </returns>
        public static String[] GetDirectories(String directoryPath)
        {
            return Directory.GetDirectories(directoryPath);
        }

        /// <summary>
        ///     获取指定文件大小
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         要获取的文件全路径
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回该文件的大小
        /// </returns>
        public static int GetFileSize(String filePath)
        {
            int _length;
            if (File.Exists(filePath))
            {
                using (FileStream tempStream = new FileStream(filePath, FileMode.Open))
                {
                    _length = (int)tempStream.Length;
                }
                return _length;
            }
            throw new System.IO.FileNotFoundException("指定的文件 : " + filePath + " 不存在...!");
        }

        /// <summary>
        /// 向当前系统中注册指定文件类型，使文件类型与对应的图标及应用程序关联起来。
        /// </summary>        
        public static void RegisterFileType(FileTypeRegInfo regInfo)
        {
            regInfo = new FileTypeRegInfo();
            regInfo.REG_Description = "KJFramework Certificate";
            regInfo.REG_IcoPath = @"D:\certificate.ico";
            regInfo.REG_ExtendName = ".cft";
            if (FileTypeRegistered(regInfo.REG_ExtendName))
            {
                return;
            }

            string relationName = regInfo.REG_ExtendName.Substring(1, regInfo.REG_ExtendName.Length - 1).ToUpper() + "_FileType";

            RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.REG_ExtendName);
            fileTypeKey.SetValue("", relationName);
            fileTypeKey.Close();

            RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            relationKey.SetValue("", regInfo.REG_Description);

            RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue("", regInfo.REG_IcoPath);

            RegistryKey shellKey = relationKey.CreateSubKey("Shell");
            RegistryKey openKey = shellKey.CreateSubKey("Open");
            RegistryKey commandKey = openKey.CreateSubKey("Command");
            commandKey.SetValue("", regInfo.REG_ExePath + " %1");

            relationKey.Close();
        }

        /// <summary>
        ///     得到指定文件类型关联信息
        /// </summary>        
        public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
        {
            if (!FileTypeRegistered(extendName))
            {
                return null;
            }

            FileTypeRegInfo regInfo = new FileTypeRegInfo(extendName);

            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName);
            regInfo.REG_Description = relationKey.GetValue("").ToString();

            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");
            regInfo.REG_IcoPath = iconKey.GetValue("").ToString();

            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command");
            string temp = commandKey.GetValue("").ToString();
            regInfo.REG_ExePath = temp.Substring(0, temp.Length - 3);

            return regInfo;
        }

        /// <summary>
        /// 更新指定文件类型关联信息
        /// </summary>    
        public static bool UpdateFileTypeRegInfo(FileTypeRegInfo regInfo)
        {
            if (!FileTypeRegistered(regInfo.REG_ExtendName))
            {
                return false;
            }
            string extendName = regInfo.REG_ExtendName;
            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName, true);
            relationKey.SetValue("", regInfo.REG_Description);

            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon", true);
            iconKey.SetValue("", regInfo.REG_IcoPath);

            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command", true);
            commandKey.SetValue("", regInfo.REG_ExePath + " %1");

            relationKey.Close();
            return true;
        }

        /// <summary>
        /// 判断当前指定文件类型是否已经注册
        /// </summary>        
        public static bool FileTypeRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     指定一个文件块大小，获取一个文件的所有文件块
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="chunkSize">文件块大小</param>
        /// <returns></returns>
        public static List<byte[]> GetFileDataChunk(String filename, int chunkSize)
        {
            if (String.IsNullOrEmpty(filename))
            {
                return null;
            }
            FileStream stream = OpenFile(filename, FileMode.Open);
            return GetFileDataChunk(stream, chunkSize);
        }

        /// <summary>
        ///     从一个IO流中按照指定的文件块大小分割出一个文件块集合
        /// </summary>
        /// <param name="stream">文件名</param>
        /// <param name="chunkSize">文件块大小</param>
        /// <returns>返回数据流集合</returns>
        public static List<byte[]> GetFileDataChunk(Stream stream, int chunkSize)
        {
            List<byte[]> datas = null;
            if (chunkSize <= 0)
            {
                chunkSize = 10240;
            }
            if (stream != null)
            {
                datas = new List<byte[]>();
                int readlength;
                byte[] data = new byte[chunkSize];
                while ((readlength = stream.Read(data, 0, data.Length)) > 0)
                {
                    if (readlength == chunkSize)
                    {
                        datas.Add(data);
                    }
                    else
                    {
                        byte[] realdata = new byte[readlength];
                        Array.ConstrainedCopy(data, 0, realdata, 0, readlength);
                        datas.Add(realdata);
                    }
                    data = new byte[chunkSize];
                }
            }
            GC.Collect();
            return datas;
        }

        /// <summary>
        ///     创建快捷方式
        ///         * 注意： 快捷方式数据集中，快捷方式地址的后缀名请加上 ".lnk"
        ///           比如： Customer.lnk (当然需要全路径 + 文件名)
        /// </summary>
        /// <param name="shortCutData">快捷方式数据集</param>
        public static void CreateShortcut(ShortCutData shortCutData) 
        {
            //WshShell shell = new WshShell();
            //IWshShortcut shortcut     = (IWshShortcut)shell.CreateShortcut(shortCutData.InkPath);
            //shortcut.TargetPath       = shortCutData.TargetPath;
            //shortcut.WorkingDirectory = shortCutData.WorkingDirectory;
            //shortcut.IconLocation     = shortCutData.IconLocation;
            //shortcut.Description      = shortCutData.Description;
            //shortcut.Arguments        = shortCutData.Arguments;
            //shortcut.Hotkey           = shortCutData.HotKey;
            //shortcut.WindowStyle      = (int)shortCutData.WindowStyle;
            //shortcut.Save();
        }

        /// <summary>
        ///     解压缩一个指定的RAR文件
        /// </summary>
        /// <param name="unRarPatch">
        ///     解压缩路径
        ///     <para>* 如果此路径不存在，则尝试创建目标路径</para>
        /// </param>
        /// <param name="rarPatch">
        ///     RAR文件路径
        ///     <para>* 此处需要文件的全路径</para>
        /// </param>
        /// <returns>返回解压缩的结果</returns>
        public static bool DecompressRAR(string unRarPatch, string rarPatch)
        {
            object obj;
            string info;
            string the_rar;
            RegistryKey reg;
            const string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            try
            {
                reg = Registry.LocalMachine.OpenSubKey(keyPath);
                if (reg == null)
                {
                    Logs.Logger.Log("Can not get registry key, #Path: " + keyPath);
                    return false;
                }
                obj = reg.GetValue("");
                the_rar = obj.ToString();
                reg.Close();
                if (!Directory.Exists(unRarPatch))
                {
                    Directory.CreateDirectory(unRarPatch);
                }
                info = "x " + rarPatch + " " + unRarPatch + " -y";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = the_rar;
                startInfo.Arguments = info;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = rarPatch;//获取压缩包路径
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }
    }

#endif


    /// <summary>
    ///     文件注册类型
    /// </summary>
    public class FileTypeRegInfo
    {
        /// <summary>
        /// 目标类型文件的扩展名
        /// </summary>
        public string REG_ExtendName;

        /// <summary>
        /// 目标文件类型说明
        /// </summary>
        public string REG_Description;

        /// <summary>
        /// 目标类型文件关联的图标
        /// </summary>
        public string REG_IcoPath;

        /// <summary>
        /// 打开目标类型文件的应用程序
        /// </summary>
        public string REG_ExePath;

        public FileTypeRegInfo()
        {
        }

        public FileTypeRegInfo(string extendName)
        {
            this.REG_ExtendName = extendName;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };
}
