// Oxygen U API
// Cracked by Payson Holmes
// Version: 8/5/22

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OxygenU
{
  internal class Pipe
  {
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WaitNamedPipe(string name, int timeout);

    public static bool NamedPipeExist(string pipeName)
    {
      bool flag;
      try
      {
        int timeout = 0;
        if (!Pipe.WaitNamedPipe(Path.GetFullPath(string.Format("\\\\.\\pipe\\{0}", (object) pipeName)), timeout))
        {
          switch (Marshal.GetLastWin32Error())
          {
            case 0:
              return false;
            case 2:
              return false;
          }
        }
        flag = true;
      }
      catch (Exception ex)
      {
        flag = false;
      }
      return flag;
    }

    public static void MainPipeClient(string pipe, string input)
    {
      if (Pipe.NamedPipeExist(pipe))
      {
        try
        {
          using (NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", pipe, PipeDirection.Out))
          {
            pipeClientStream.Connect();
            using (StreamWriter streamWriter = new StreamWriter((Stream) pipeClientStream))
            {
              streamWriter.Write(input);
              streamWriter.Dispose();
            }
            pipeClientStream.Dispose();
          }
        }
        catch (IOException ex)
        {
          int num = (int) MessageBox.Show("Pipe Is Wrong! | Debug: " + ex.Message, "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message.ToString());
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("Error occured, Please Disable Your Antivirus!", "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public class BasicInject
    {
      [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
      internal static extern IntPtr LoadLibraryA(string lpFileName);

      [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
      internal static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FreeLibrary(IntPtr hModule);

      [DllImport("kernel32.dll")]
      internal static extern IntPtr OpenProcess(
        Pipe.BasicInject.ProcessAccess dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
        int dwProcessId);

      [DllImport("kernel32.dll", SetLastError = true)]
      internal static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        uint flAllocationType,
        uint flProtect);

      [DllImport("kernel32.dll", SetLastError = true)]
      internal static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        uint nSize,
        out UIntPtr lpNumberOfBytesWritten);

      [DllImport("kernel32.dll")]
      internal static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        UIntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out IntPtr lpThreadId);

      [DllImport("kernel32.dll", SetLastError = true)]
      internal static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [Out] byte[] lpBuffer,
        int dwSize,
        out int lpNumberOfBytesRead);

      public bool InjectDLL()
      {
        if (Process.GetProcessesByName("RobloxPlayerBeta").Length == 0)
          return false;
        Process process = Process.GetProcessesByName("RobloxPlayerBeta")[0];
        byte[] bytes = new ASCIIEncoding().GetBytes(AppDomain.CurrentDomain.BaseDirectory + Client.name);
        IntPtr hModule = Pipe.BasicInject.LoadLibraryA("kernel32.dll");
        UIntPtr procAddress = Pipe.BasicInject.GetProcAddress(hModule, "LoadLibraryA");
        Pipe.BasicInject.FreeLibrary(hModule);
        if (procAddress == UIntPtr.Zero)
          return false;
        IntPtr hProcess = Pipe.BasicInject.OpenProcess(Pipe.BasicInject.ProcessAccess.AllAccess, false, process.Id);
        if (hProcess == IntPtr.Zero)
          return false;
        IntPtr num = Pipe.BasicInject.VirtualAllocEx(hProcess, (IntPtr) 0, (uint) bytes.Length, 12288U, 4U);
        return !(num == IntPtr.Zero) && Pipe.BasicInject.WriteProcessMemory(hProcess, num, bytes, (uint) bytes.Length, out UIntPtr _) && !(Pipe.BasicInject.CreateRemoteThread(hProcess, (IntPtr) 0, 0U, procAddress, num, 0U, out IntPtr _) == IntPtr.Zero);
      }

      [Flags]
      public enum ProcessAccess
      {
        AllAccess = 1050235, // 0x0010067B
        CreateThread = 2,
        DuplicateHandle = 64, // 0x00000040
        QueryInformation = 1024, // 0x00000400
        SetInformation = 512, // 0x00000200
        Terminate = 1,
        VMOperation = 8,
        VMRead = 16, // 0x00000010
        VMWrite = 32, // 0x00000020
        Synchronize = 1048576, // 0x00100000
      }
    }
  }
}
