// Oxygen U API
// Cracked by Payson Holmes
// Version: 8/5/22

using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace OxygenU
{
  public class Client
  {
    public static string pipe = "OxygenU";
    public static string name = "OxygenBytecode.dll";
    public static string downloadLink = "https://github.com/P-DennyGamingYT/OxygenU-API/raw/main/OxygenBytecode.dll";
    private Pipe.BasicInject Injector = new Pipe.BasicInject();

    public void Execute(string script) => Pipe.MainPipeClient(Client.pipe, script);

    public void IntializeUpdate()
    {
      if (System.IO.File.Exists(Client.name))
      {
        if (!this.isOXygenUAttached())
        {
          try
          {
            System.IO.File.Delete(Client.name);
            new WebClient().DownloadFile(Client.downloadLink, Client.name);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(" Debug: " + ex.Message, "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("Roblox Is Running And Is Attached, Roblox Process Will Be Killed And Get You The Latest Update.", "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.KillRoblox();
          if (System.IO.File.Exists(Client.name))
          {
            try
            {
              System.IO.File.Delete(Client.name);
              new WebClient().DownloadFile(Client.downloadLink, Client.name);
            }
            catch (Exception ex)
            {
              int num2 = (int) MessageBox.Show(" Debug: " + ex.Message, "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
        }
      }
      else
      {
        try
        {
          new WebClient().DownloadFile(Client.downloadLink, Client.name);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Debug: " + ex.Message, "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    public void Attach()
    {
      if (Process.GetProcessesByName("RobloxPlayerBeta").Length == 1)
      {
        if (!this.isOXygenUAttached())
        {
          if (!System.IO.File.Exists(Client.name))
            this.IntializeUpdate();
          this.Injector.InjectDLL();
        }
        else
        {
          int num1 = (int) MessageBox.Show("Oxygen U Has Been Already Attached .", "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("Roblox Is Not Running!", "Oxygen U", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public bool isRobloxOn() => Process.GetProcessesByName("RobloxPlayerBeta").Length == 1;

    public bool isOXygenUAttached() => Pipe.NamedPipeExist(Client.pipe);

    public void KillRoblox()
    {
      foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
        process.Kill();
    }
  }
}
