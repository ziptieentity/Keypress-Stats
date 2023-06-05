using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Diagnostics;
using Gma.System.MouseKeyHook;

namespace KeyPressStats
{
    public partial class Form1 : Form
    {
        public string KeypressessPath = Application.StartupPath + "/Keypresses.json";
        public string ImagesPath = Application.StartupPath + "/Icons";

        public Dictionary<string, int> keypresses = new Dictionary<string, int>();
        IKeyboardMouseEvents globalKeyHook;
        ToolStripItem mostUsedKey;
        ToolStripItem leastUsedKey;
        ToolStripItem nextUpdate;
        DateTime nextUpdateTime;

        public Form1()
        {
            InitializeComponent();

            EnsureKeypressFile();

            nextUpdateTime = DateTime.Now.AddMilliseconds(SaveInterval.Interval);
        }

        private void EnsureKeypressFile()
        {
            if (!File.Exists(KeypressessPath))
            {
                File.WriteAllText(KeypressessPath, "{}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SaveInterval.Start();

                Subscribe();
                Hide();

                Tray.Visible = true;
                Tray.Text = "Keypress Stats";
                Tray.ContextMenuStrip = new ContextMenuStrip();

                mostUsedKey = Tray.ContextMenuStrip.Items.Add("Most Used Key: None", null);
                mostUsedKey.Enabled = false;

                leastUsedKey = Tray.ContextMenuStrip.Items.Add("Least Used Key: None", null);
                leastUsedKey.Enabled = false;

                nextUpdate = Tray.ContextMenuStrip.Items.Add("Next Update: ", null);
                nextUpdate.Enabled = false;

                var version = Tray.ContextMenuStrip.Items.Add($"Version: v{Application.ProductVersion}");
                version.Enabled = false;

                Tray.ContextMenuStrip.Items.Add("-");

                Tray.ContextMenuStrip.Items.Add("Save Keypresses", Image.FromFile(ImagesPath + "/save_32.png"), SaveCommand);
                Tray.ContextMenuStrip.Items.Add("Open Keypresses File", Image.FromFile(ImagesPath + "/folder_32.png"), OpenKeypressessFileCommand);
                Tray.ContextMenuStrip.Items.Add("Exit", Image.FromFile(ImagesPath + "/close_32.png"), ExitCommand);

                ShowInTaskbar = false;
                SaveKeypresses(true);
                UpdateTimer.Start();
            }
            catch (Exception ex) { MessageBox.Show($"A fatal error has occured: {ex.Message}. Please report this to the creator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); Clipboard.SetText(ex.Message); Application.Exit(); }
        }
        private void Subscribe()
        {
            globalKeyHook = Hook.GlobalEvents();
            globalKeyHook.KeyDown += (o, e) =>
            {
                AddNewKeypress(e.KeyCode.ToString());
                nextUpdateTime = DateTime.Now.AddMilliseconds(SaveInterval.Interval);
            };
        }

        private void OpenKeypressessFileCommand(object sender, EventArgs e)
        {
            try
            {
                Process.Start(KeypressessPath);
            }
            catch
            {
                Process.Start("notepad.exe", KeypressessPath);
            }
        }

        private void AddNewKeypress(string keycode)
        {
            if(keypresses.ContainsKey(keycode)) { keypresses[keycode]++; return; }
            keypresses.Add(keycode, 1);
        }

        private void SaveKeypresses(bool inital)
        {
            try
            {
                if (keypresses.Count == 0 && inital == false) return;

                var currentKeypresses = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(KeypressessPath));

                foreach (var item in keypresses)
                {
                    if (currentKeypresses.ContainsKey(item.Key)) { currentKeypresses[item.Key]+=item.Value; continue; }
                    currentKeypresses.Add(item.Key, item.Value);
                }

                var sortedDict = SortDictionaryByValues(currentKeypresses);

                if (mostUsedKey != null && sortedDict.Count > 0)
                    mostUsedKey.Text = $"Most Used Key: {sortedDict.Last().Key} - {sortedDict.Last().Value}";
                if (leastUsedKey != null && sortedDict.Count > 0)
                    leastUsedKey.Text = $"Least Used Key: {sortedDict.First().Key} - {sortedDict.First().Value}";

                var json = JsonConvert.SerializeObject(sortedDict, Formatting.Indented);
                File.WriteAllText(Application.StartupPath + "./Keypresses.json", json);
                currentKeypresses.Clear();
                keypresses.Clear();
#if DEBUG
                Tray.ShowBalloonTip(2500, "Saved", "Keypresses have been saved", ToolTipIcon.Info);
#endif
            }
            catch (Exception ex) { MessageBox.Show($"A fatal error has occured: {ex.Message}. Please report this to the creator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); Clipboard.SetText(ex.Message); Application.Exit(); }
        }
        private void ExitCommand(object sender, EventArgs e)
        {
            Close();
        }
        private void SaveCommand(object sender, EventArgs e)
        {
            SaveKeypresses(false);
            nextUpdateTime = DateTime.Now.AddMilliseconds(SaveInterval.Interval);
        }
        private void SaveTimerTick(object sender, EventArgs e)
        {
            SaveKeypresses(false);
            nextUpdateTime = DateTime.Now.AddMilliseconds(SaveInterval.Interval);
        }

        public static Dictionary<string, int> SortDictionaryByValues(Dictionary<string, int> dict)
        {
            var sortedEntries = new List<KeyValuePair<string, int>>(dict);

            sortedEntries.Sort((x, y) => x.Value.CompareTo(y.Value));

            var sortedDict = new Dictionary<string, int>();

            foreach (var entry in sortedEntries)
            {
                sortedDict.Add(entry.Key, entry.Value);
            }

            return sortedDict;
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            if (nextUpdate == null)
                return;

            nextUpdate.Text = $"Next Update: {(nextUpdateTime - DateTime.Now).Seconds} seconds";
        }

        private void OnExit(object sender, FormClosingEventArgs e)
        {
            SaveInterval.Stop();
            UpdateTimer.Stop();
            SaveKeypresses(false);
        }
    }
}