using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralWinForm.Tasks
{
    public class IncrementTask
    {
        private MainForm _mainForm;
        public int InitNumber { get; set; } = 0;
        public int StepNumber { get; set; } = 1;
        public IncrementTask(MainForm mainForm)
        {
            _mainForm = mainForm;
        }
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var now = InitNumber;
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // 使用Task.Run在后台线程执行计算
                    var newNumber = await Task.Run(() =>
                    {
                        // 模拟一些计算
                        return now += StepNumber;
                    }, cancellationToken);

                    // 更新UI（await后自动回到UI线程）
                    _mainForm.label_increment_now.Text = $"当前: {newNumber}";

                    // 添加延迟，避免UI更新太快
                    await Task.Delay(500, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // 任务被取消
                Console.WriteLine("递增任务被取消");
            }
        }
    }
}
