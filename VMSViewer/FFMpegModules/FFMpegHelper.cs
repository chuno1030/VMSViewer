﻿using System;
using System.IO;
using System.Runtime.InteropServices;

using FFmpeg.AutoGen;

namespace VMSViewer
{
    /// <summary>
    /// FFMPEG 헬퍼
    /// </summary>
    public static class FFMpegHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Import
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region DLL 디렉토리 설정하기 - SetDllDirectory(directoryPath)

        /// <summary>
        /// DLL 디렉토리 설정하기
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        /// <returns>처리 결과</returns>
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetDllDirectory(string directoryPath);

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// LD_LIBRARY_PATH
        /// </summary>
        private const string LD_LIBRARY_PATH = "LD_LIBRARY_PATH";

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region 등록하기 - Register()

        /// <summary>
        /// 등록하기
        /// </summary>
        public static void Register()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    {
                        string currentDirectoryPath = Environment.CurrentDirectory;

                        while (currentDirectoryPath != null)
                        {
                            string dllDirectoryPath = Path.Combine(currentDirectoryPath, "FFMpegDLL");

                            if (Directory.Exists(dllDirectoryPath))
                            {
                                Register(dllDirectoryPath);

                                return;
                            }

                            currentDirectoryPath = Directory.GetParent(currentDirectoryPath)?.FullName;
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        string dllDirectoryPath = Environment.GetEnvironmentVariable(LD_LIBRARY_PATH);

                        Register(dllDirectoryPath);

                        break;
                    }
            }
        }

        #endregion

        #region 에러 메시지 구하기 - GetErrorMessage(errorCode)

        /// <summary>
        /// 에러 메시지 구하기
        /// </summary>
        /// <param name="errorCode">에러 코드</param>
        /// <returns>에러 메시지</returns>
        public static unsafe string GetErrorMessage(int errorCode)
        {
            int bufferSize = 1024;

            byte* buffer = stackalloc byte[bufferSize];

            ffmpeg.av_strerror(errorCode, buffer, (ulong)bufferSize);

            string message = Marshal.PtrToStringAnsi((IntPtr)buffer);

            return message;
        }

        #endregion
        #region 에러시 예외 던지기 - ThrowExceptionIfError(error)

        /// <summary>
        /// 에러시 예외 던지기
        /// </summary>
        /// <param name="errorCode">에러 코드</param>
        /// <returns>에러 코드</returns>
        public static int ThrowExceptionIfError(this int errorCode)
        {
            if (errorCode < 0)
            {
                throw new ApplicationException(GetErrorMessage(errorCode));
            }

            return errorCode;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Private

        #region 등록하기 - Register(dllDirectoryPath)

        /// <summary>
        /// 등록하기
        /// </summary>
        /// <param name="dllDirectoryPath">DLL 디렉토리 경로</param>
        private static void Register(string dllDirectoryPath)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:

                    SetDllDirectory(dllDirectoryPath);
                    break;

                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    string currentValue = Environment.GetEnvironmentVariable(LD_LIBRARY_PATH);

                    if (string.IsNullOrWhiteSpace(currentValue) == false && currentValue.Contains(dllDirectoryPath) == false)
                    {
                        string newValue = currentValue + Path.PathSeparator + dllDirectoryPath;

                        Environment.SetEnvironmentVariable(LD_LIBRARY_PATH, newValue);
                    }
                    break;
            }
        }

        #endregion

        public static bool FFMpegInitialize()
        {
            try
            {
                string FFMpegDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FFMpegDLL");

                if (Directory.Exists(FFMpegDirectory) == false)
                    throw new Exception("FFMpeg Directory 미존재");

                if (SetDllDirectory(FFMpegDirectory) == false)
                    throw new Exception("Failed FFMpegInitialize");

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }
    }
}