using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuSchedulingWinForms
{
    // Helper class for MLFQ
    public class Process
    {
        public int Id { get; set; }
        public double ArrivalTime { get; set; }
        public double BurstTime { get; set; }
        public double RemainingTime { get; set; }
        public double WaitingTime { get; set; }
        public double TurnaroundTime { get; set; }
        public double CompletionTime { get; set; }
        public int CurrentQueue { get; set; } // 0, 1, 2 for MLFQ
        public double TimeInCurrentQuantum { get; set; } // For RR queues in MLFQ
        public bool IsCompleted { get; set; }

        public Process(int id, double at, double bt)
        {
            Id = id;
            ArrivalTime = at;
            BurstTime = bt;
            RemainingTime = bt;
            IsCompleted = false;
            CurrentQueue = 0; // Start in highest priority queue for MLFQ
            TimeInCurrentQuantum = 0;
            WaitingTime = 0;
            TurnaroundTime = 0;
            CompletionTime = 0;
        }
    }

    public static class Algorithms
    {
        public static void fcfsAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            int npX2 = np * 2;
            double[] bp = new double[np];    // Burst times
            double[] wtp = new double[np];   // Waiting times
            double[] ct = new double[np];    // Completion times
            string[] output1 = new string[npX2];
            double twt = 0.0, awt;
            int num;
            DialogResult result = MessageBox.Show("First Come First Serve Scheduling ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                for (num = 0; num <= np - 1; num++)
                {
                    string input =
                    Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ",
                                                       "Burst time for P" + (num + 1),
                                                       "", -1, -1);
                    bp[num] = Convert.ToInt64(input);
                }

                double currentTime = 0;  // Track current time to calculate completion times

                for (num = 0; num <= np - 1; num++)
                {
                    if (num == 0)
                    {
                        wtp[num] = 0;
                    }
                    else
                    {
                        wtp[num] = wtp[num - 1] + bp[num - 1];
                        MessageBox.Show("Waiting time for P" + (num + 1) + " = " + wtp[num], "Job Queue", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }

                    // Calculate completion time for each process
                    currentTime = wtp[num] + bp[num];
                    ct[num] = currentTime;
                }

                for (num = 0; num <= np - 1; num++)
                {
                    twt = twt + wtp[num];
                }
                awt = twt / np;
                MessageBox.Show("Average waiting time for " + np + " processes" + " = " + awt + " sec(s)", "Average Awaiting Time", MessageBoxButtons.OK, MessageBoxIcon.None);

                // Calculate throughput
                double totalTime = ct[np - 1];  // Total time is the completion time of the last process
                double throughput = np / totalTime;

                // Display throughput
                MessageBox.Show($"Throughput: {throughput:F5} processes/time unit\n" +
                                $"({np} processes / {totalTime} time units)",
                                "FCFS Throughput", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == DialogResult.No)
            {
                //this.Hide();
                //Form1 frm = new Form1();
                //frm.ShowDialog();
            }
        }

        public static void sjfAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            double[] bp = new double[np];
            double[] wtp = new double[np];
            double[] p = new double[np];
            double twt = 0.0, awt; 
            int x, num;
            double temp = 0.0;
            bool found = false;

            DialogResult result = MessageBox.Show("Shortest Job First Scheduling", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                for (num = 0; num <= np - 1; num++)
                {
                    string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                           "Burst time for P" + (num + 1),
                                                           "",
                                                           -1, -1);

                    bp[num] = Convert.ToInt64(input);
                }
                for (num = 0; num <= np - 1; num++)
                {
                    p[num] = bp[num];
                }
                for (x = 0; x <= np - 2; x++)
                {
                    for (num = 0; num <= np - 2; num++)
                    {
                        if (p[num] > p[num + 1])
                        {
                            temp = p[num];
                            p[num] = p[num + 1];
                            p[num + 1] = temp;
                        }
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    if (num == 0)
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (p[num] == bp[x] && found == false)
                            {
                                wtp[num] = 0;
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time:", MessageBoxButtons.OK, MessageBoxIcon.None);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                bp[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                    else
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (p[num] == bp[x] && found == false)
                            {
                                wtp[num] = wtp[num - 1] + p[num - 1];
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time", MessageBoxButtons.OK, MessageBoxIcon.None);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                bp[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    twt = twt + wtp[num];
                }
                MessageBox.Show("Average waiting time for " + np + " processes" + " = " + (awt = twt / np) + " sec(s)", "Average waiting time", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void priorityAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            DialogResult result = MessageBox.Show("Priority Scheduling ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                double[] bp = new double[np];
                double[] wtp = new double[np + 1];
                int[] p = new int[np];
                int[] sp = new int[np];
                int x, num;
                double twt = 0.0;
                double awt;
                int temp = 0;
                bool found = false;
                for (num = 0; num <= np - 1; num++)
                {
                    string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                           "Burst time for P" + (num + 1),
                                                           "",
                                                           -1, -1);

                    bp[num] = Convert.ToInt64(input);
                }
                for (num = 0; num <= np - 1; num++)
                {
                    string input2 =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter priority: ",
                                                           "Priority for P" + (num + 1),
                                                           "",
                                                           -1, -1);

                    p[num] = Convert.ToInt16(input2);
                }
                for (num = 0; num <= np - 1; num++)
                {
                    sp[num] = p[num];
                }
                for (x = 0; x <= np - 2; x++)
                {
                    for (num = 0; num <= np - 2; num++)
                    {
                        if (sp[num] > sp[num + 1])
                        {
                            temp = sp[num];
                            sp[num] = sp[num + 1];
                            sp[num + 1] = temp;
                        }
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    if (num == 0)
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (sp[num] == p[x] && found == false)
                            {
                                wtp[num] = 0;
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time", MessageBoxButtons.OK);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                temp = x;
                                p[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                    else
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (sp[num] == p[x] && found == false)
                            {
                                wtp[num] = wtp[num - 1] + bp[temp];
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time", MessageBoxButtons.OK);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                temp = x;
                                p[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    twt = twt + wtp[num];
                }
                MessageBox.Show("Average waiting time for " + np + " processes" + " = " + (awt = twt / np) + " sec(s)", "Average waiting time", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Console.WriteLine("\n\nAverage waiting time: " + (awt = twt / np));
                //Console.ReadLine();
            }
            else
            {
                //this.Hide();
            }
        }

        public static void roundRobinAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            int i, counter = 0;
            double total = 0.0;
            double timeQuantum;
            double waitTime = 0, turnaroundTime = 0;
            double averageWaitTime, averageTurnaroundTime;
            double[] arrivalTime = new double[10];
            double[] burstTime = new double[10];
            double[] temp = new double[10];
            int x = np;

            DialogResult result = MessageBox.Show("Round Robin Scheduling", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                for (i = 0; i < np; i++)
                {
                    string arrivalInput =
                            Microsoft.VisualBasic.Interaction.InputBox("Enter arrival time: ",
                                                               "Arrival time for P" + (i + 1),
                                                               "",
                                                               -1, -1);

                    arrivalTime[i] = Convert.ToInt64(arrivalInput);

                    string burstInput =
                            Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                               "Burst time for P" + (i + 1),
                                                               "",
                                                               -1, -1);

                    burstTime[i] = Convert.ToInt64(burstInput);

                    temp[i] = burstTime[i];
                }
                string timeQuantumInput =
                            Microsoft.VisualBasic.Interaction.InputBox("Enter time quantum: ", "Time Quantum",
                                                               "",
                                                               -1, -1);

                timeQuantum = Convert.ToInt64(timeQuantumInput);
                Helper.QuantumTime = timeQuantumInput;

                for (total = 0, i = 0; x != 0;)
                {
                    if (temp[i] <= timeQuantum && temp[i] > 0)
                    {
                        total = total + temp[i];
                        temp[i] = 0;
                        counter = 1;
                    }
                    else if (temp[i] > 0)
                    {
                        temp[i] = temp[i] - timeQuantum;
                        total = total + timeQuantum;
                    }
                    if (temp[i] == 0 && counter == 1)
                    {
                        x--;
                        //printf("nProcess[%d]tt%dtt %dttt %d", i + 1, burst_time[i], total - arrival_time[i], total - arrival_time[i] - burst_time[i]);
                        MessageBox.Show("Turnaround time for Process " + (i + 1) + " : " + (total - arrivalTime[i]), "Turnaround time for Process " + (i + 1), MessageBoxButtons.OK);
                        MessageBox.Show("Wait time for Process " + (i + 1) + " : " + (total - arrivalTime[i] - burstTime[i]), "Wait time for Process " + (i + 1), MessageBoxButtons.OK);
                        turnaroundTime = (turnaroundTime + total - arrivalTime[i]);
                        waitTime = (waitTime + total - arrivalTime[i] - burstTime[i]);                        
                        counter = 0;
                    }
                    if (i == np - 1)
                    {
                        i = 0;
                    }
                    else if (arrivalTime[i + 1] <= total)
                    {
                        i++;
                    }
                    else
                    {
                        i = 0;
                    }
                }
                averageWaitTime = Convert.ToInt64(waitTime * 1.0 / np);
                averageTurnaroundTime = Convert.ToInt64(turnaroundTime * 1.0 / np);
                MessageBox.Show("Average wait time for " + np + " processes: " + averageWaitTime + " sec(s)", "", MessageBoxButtons.OK);
                MessageBox.Show("Average turnaround time for " + np + " processes: " + averageTurnaroundTime + " sec(s)", "", MessageBoxButtons.OK);
            }
        }
        public static void srtfAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            if (np <= 0)
            {
                MessageBox.Show("Number of processes must be positive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Shortest Remaining Time First (Preemptive SJF) Scheduling", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                double[] arrivalTime = new double[np];
                double[] burstTime = new double[np];
                double[] remainingTime = new double[np];
                double[] waitingTime = new double[np];
                double[] turnaroundTime = new double[np];
                double[] completionTime = new double[np];
                int[] processId = new int[np]; 
                bool[] isCompleted = new bool[np];

                for (int i = 0; i < np; i++)
                {
                    processId[i] = i + 1;
                    string arrivalInput = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Enter Arrival time for P{i + 1}:", "Arrival Time", "0", -1, -1);
                    arrivalTime[i] = Convert.ToDouble(arrivalInput);

                    string burstInput = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Enter Burst time for P{i + 1}:", "Burst Time", "1", -1, -1);
                    burstTime[i] = Convert.ToDouble(burstInput);
                    if (burstTime[i] <= 0)
                    {
                        MessageBox.Show($"Burst time for P{i + 1} must be positive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    remainingTime[i] = burstTime[i];
                    isCompleted[i] = false;
                }

                double currentTime = 0;
                int completedCount = 0;
                double totalWaitingTime = 0;
                double totalTurnaroundTime = 0;

                // Main simulation loop
                while (completedCount < np)
                {
                    int shortestJobIndex = -1;
                    double minRemainingTime = double.MaxValue;

                    // Find the process with the minimum remaining time among arrived and incomplete processes
                    for (int i = 0; i < np; i++)
                    {
                        if (arrivalTime[i] <= currentTime && !isCompleted[i] && remainingTime[i] < minRemainingTime)
                        {
                            minRemainingTime = remainingTime[i];
                            shortestJobIndex = i;
                        }
                        //If remaining times are equal choose the one that arrived earlier
                        else if (arrivalTime[i] <= currentTime && !isCompleted[i] && remainingTime[i] == minRemainingTime)
                        {
                            if (arrivalTime[i] < arrivalTime[shortestJobIndex])
                            {
                                shortestJobIndex = i; // Choose earlier arrival
                            }
                        }
                    }

                    // If no process is ready (idle time)
                    if (shortestJobIndex == -1)
                    {
                        currentTime++;
                    }
                    // Process the shortest job for one time unit
                    else
                    {
                        remainingTime[shortestJobIndex]--;
                        currentTime++;

                        // Check if the process completed
                        if (remainingTime[shortestJobIndex] == 0)
                        {
                            completionTime[shortestJobIndex] = currentTime;
                            turnaroundTime[shortestJobIndex] = completionTime[shortestJobIndex] - arrivalTime[shortestJobIndex];
                            waitingTime[shortestJobIndex] = turnaroundTime[shortestJobIndex] - burstTime[shortestJobIndex];

                            // Ensure waiting time isn't negative
                            if (waitingTime[shortestJobIndex] < 0) waitingTime[shortestJobIndex] = 0;

                            totalWaitingTime += waitingTime[shortestJobIndex];
                            totalTurnaroundTime += turnaroundTime[shortestJobIndex];
                            isCompleted[shortestJobIndex] = true;
                            completedCount++;

                            // Display results
                            MessageBox.Show($"Process P{processId[shortestJobIndex]} completed.\n" +
                                            $"Completion Time: {completionTime[shortestJobIndex]}\n" +
                                            $"Turnaround Time: {turnaroundTime[shortestJobIndex]}\n" +
                                            $"Waiting Time: {waitingTime[shortestJobIndex]}",
                                            $"P{processId[shortestJobIndex]} Stats", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                } 

                double avgWaitingTime = totalWaitingTime / np;
                double avgTurnaroundTime = totalTurnaroundTime / np;

                MessageBox.Show($"Average Waiting Time for {np} processes = {avgWaitingTime:F2} sec(s)",
                                "SRTF Average Waiting Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show($"Average Turnaround Time for {np} processes = {avgTurnaroundTime:F2} sec(s)",
                                "SRTF Average Turnaround Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // No was clicked
            }
        }
        public static void mlfqAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            if (np <= 0)
            {
                MessageBox.Show("Number of processes must be positive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Define MLFQ Parameters ---
            int numQueues = 3;
            double[] quantum = { 8, 16, double.MaxValue }; // MaxValue for FCFS in last queue
            // Queue 0: RR, Q=8
            // Queue 1: RR, Q=16
            // Queue 2: FCFS

            string rules = $"Multi-Level Feedback Queue Scheduling\n" +
                           $"Rules:\n" +
                           $"- {numQueues} Queues (0=Highest Priority)\n" +
                           $"- Q0: RR (Quantum = {quantum[0]})\n" +
                           $"- Q1: RR (Quantum = {quantum[1]})\n" +
                           $"- Q2: FCFS\n" +
                           $"- Preemption: Yes\n" +
                           $"- Demotion: On full quantum use (Q0->Q1, Q1->Q2)\n" +
                           $"- Promotion/Aging: No";

            DialogResult result = MessageBox.Show(rules, "MLFQ Simulation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                List<Process> processes = new List<Process>();
                List<Queue<Process>> queues = new List<Queue<Process>>(numQueues);
                for (int i = 0; i < numQueues; i++)
                {
                    queues.Add(new Queue<Process>());
                }

                // Get Process Details
                for (int i = 0; i < np; i++)
                {
                    string arrivalInput = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Enter Arrival time for P{i + 1}:", "Arrival Time", "0", -1, -1);
                    double at = Convert.ToDouble(arrivalInput);

                    string burstInput = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Enter Burst time for P{i + 1}:", "Burst Time", "1", -1, -1);
                    double bt = Convert.ToDouble(burstInput);
                    if (bt <= 0)
                    {
                        MessageBox.Show($"Burst time for P{i + 1} must be positive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    processes.Add(new Process(i + 1, at, bt));
                }

                // Sort processes by arrival time initially to handle arrivals correctly
                processes = processes.OrderBy(p => p.ArrivalTime).ToList();
                int processesEnteredSystem = 0;

                double currentTime = 0;
                int completedCount = 0;
                Process currentProcess = null;
                int currentRunningQueue = -1; 

                // Main loop
                while (completedCount < np)
                {
                    // Check for new process arrivals and add to the highest priority queue
                    while (processesEnteredSystem < np && processes[processesEnteredSystem].ArrivalTime <= currentTime)
                    {
                        processes[processesEnteredSystem].CurrentQueue = 0; // Start in Q0
                        queues[0].Enqueue(processes[processesEnteredSystem]);
                        processesEnteredSystem++;
                    }

                    // Select process to run from the highest priority non-empty queue
                    // Only select if CPU is idle or process finished
                    if (currentProcess == null || currentProcess.IsCompleted) 
                    {
                        currentProcess = null; // Reset
                        currentRunningQueue = -1;
                        for (int i = 0; i < numQueues; i++)
                        {
                            if (queues[i].Count > 0)
                            {
                                currentProcess = queues[i].Dequeue();
                                currentRunningQueue = i;
                                currentProcess.TimeInCurrentQuantum = 0; // Reset quantum timer when selected
                                break; // Found a process, stop searching lower queues
                            }
                        }
                    }

                    //increment time (CPU Idle)
                    if (currentProcess == null)
                    {
                        // Check if there are still processes that haven't arrived yet
                        bool futureArrivals = processesEnteredSystem < np;
                        bool processesWaiting = queues.Any(q => q.Count > 0);

                        if (futureArrivals || processesWaiting) // Only advance time if there's potential work
                        {
                            currentTime++;
                        }
                        else if (completedCount < np)
                        {
                            currentTime++;
                        }
                        continue; // Go back to check for arrivals/select process
                    }

                    // Run process for one time unit/until quantum expires/completion)
                    currentTime++;
                    currentProcess.RemainingTime--;
                    currentProcess.TimeInCurrentQuantum++;

                    // Check process completion
                    if (currentProcess.RemainingTime == 0)
                    {
                        currentProcess.IsCompleted = true;
                        currentProcess.CompletionTime = currentTime;
                        currentProcess.TurnaroundTime = currentProcess.CompletionTime - currentProcess.ArrivalTime;
                        currentProcess.WaitingTime = currentProcess.TurnaroundTime - currentProcess.BurstTime;
                        if (currentProcess.WaitingTime < 0) currentProcess.WaitingTime = 0;

                        completedCount++;

                        // Display individual results (optional)
                        MessageBox.Show($"Process P{currentProcess.Id} completed.\n" +
                                        $"Final Queue: Q{currentRunningQueue}\n" +
                                        $"Completion Time: {currentProcess.CompletionTime}\n" +
                                        $"Turnaround Time: {currentProcess.TurnaroundTime}\n" +
                                        $"Waiting Time: {currentProcess.WaitingTime}",
                                        $"P{currentProcess.Id} Stats (MLFQ)", MessageBoxButtons.OK, MessageBoxIcon.None);
                        //free CPU
                        currentProcess = null;
                        currentRunningQueue = -1;
                    }
                    // Check if quantum is used
                    else if (currentRunningQueue < numQueues - 1 && currentProcess.TimeInCurrentQuantum >= quantum[currentRunningQueue])
                    {
                        // Demote the process
                        int nextQueue = currentRunningQueue + 1;
                        currentProcess.CurrentQueue = nextQueue;
                        currentProcess.TimeInCurrentQuantum = 0;
                        // Add to the end of the lower queue
                        queues[nextQueue].Enqueue(currentProcess); 

                        currentProcess = null; // CPU becomes free
                        currentRunningQueue = -1;
                    }

                } 

                // Calculate and display averages including throughput
                double totalWaitingTime = processes.Sum(p => p.WaitingTime);
                double totalTurnaroundTime = processes.Sum(p => p.TurnaroundTime);
                double avgWaitingTime = totalWaitingTime / np;
                double avgTurnaroundTime = totalTurnaroundTime / np;
                double maxCompletionTime = processes.Max(p => p.CompletionTime);
                double throughput = np / maxCompletionTime;

                MessageBox.Show($"Average Waiting Time for {np} processes = {avgWaitingTime:F2} sec(s)",
                                "MLFQ Average Waiting Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show($"Average Turnaround Time for {np} processes = {avgTurnaroundTime:F2} sec(s)",
                                "MLFQ Average Turnaround Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show($"Throughput: {throughput:F5} processes/time unit\n" +
                                $"({np} processes / {maxCompletionTime} time units)",
                                "MLFQ Throughput", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // User clicked No
            }
        }

    }
    /*
    public static class Helper
    {
        public static string QuantumTime { get; set; } = "-"; // Default value
    }
    */
}

