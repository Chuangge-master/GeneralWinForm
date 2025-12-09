using GeneralWinForm.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralWinForm
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        // 保存上次的值
        private int _lastIncrementValue = 0;
        private int _lastDecrementValue = 100;


        public MainForm()
        {
            InitializeComponent();

        }

        // 确保在窗体关闭时清理资源
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            base.OnFormClosing(e);
        }

        async private void StartButton_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _isRunning = true;
                StartButton.Enabled = false;
                StopButton.Enabled = true;
            }

            
            // 启动两个并行任务
            var incrementTask = new IncrementTask(this) { 
                InitNumber = Convert.ToInt32(txt_increment_init.Text),
                StepNumber = Convert.ToInt32(txt_increment_step.Text)
            }.RunAsync(_cancellationTokenSource.Token);

            var decrementTask = new DecrementTask(this)
            {
                InitNumber = Convert.ToInt32(txt_decrement_init.Text),
                StepNumber = Convert.ToInt32(txt_decrement_step.Text)
            }.RunAsync(_cancellationTokenSource.Token);

            // 等待两个任务完成
            await Task.WhenAll(incrementTask, decrementTask);

            
            _isRunning = false;
            StopButton.Enabled = false;
            StartButton.Enabled = true;
            
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _isRunning = false;
            StopButton.Enabled = false;
            StartButton.Enabled = true;
        }
    }
}
