using System;
using System.Threading;
using System.Threading.Tasks;

namespace PatientManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var manager = new PatientInfo();
            var testDate = new DateTime(2017, 01, 01);

            CalculateTheExecutionTime(() => manager.GetPatientsFirstCase(testDate));
            CalculateTheExecutionTime(() => manager.GetPatientsSecondCase(testDate));
            CalculateTheExecutionTime(() => manager.GetPatientsThirdCase(testDate));
        }

        public static void CalculateTheExecutionTime(Action action)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            cancellationToken.Register(() => action());
            tokenSource.CancelAfter(10000);

            var start = DateTime.Now;

            var task = Task.Run(() => action.Invoke(), cancellationToken);

            try
            {
                Task.WaitAny(new[] { task }, cancellationToken);
                Console.WriteLine($"{action.Method.Name} : {DateTime.Now - start}");
                Console.WriteLine("-------");
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{action.Method.Name} is too slow. Time has passed {DateTime.Now - start}");
                Console.WriteLine(e.Message);
                Console.WriteLine("-------");
            }
        }
    }
}
