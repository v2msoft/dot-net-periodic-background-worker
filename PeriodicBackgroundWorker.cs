using System;
using System.Threading;
using System.Timers;

namespace V2MSoftware.Threading {

    abstract class PeriodicBackgroundWorker {

        #region Private Properties
        private System.Timers.Timer Timer { get; set; }
        private AutoResetEvent SyncMutex = new AutoResetEvent(true);
        #endregion

        /// <summary>
        /// Class constructor. It allocates the memory for the background timer and
        /// sets its interval period.
        /// IMPORTANT: This method will not start the timer execution so the Start()
        /// method must be called to start the task execution.
        /// </summary>
        /// <param name="period">The amount of time (in milliseconds) that have to be elapsed between two 
        /// executions of the DoWork method.</param>
        public PeriodicBackgroundWorker(double period) {
            this.Timer = new System.Timers.Timer();
            this.Timer.Interval = period;
            this.Timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Wraps the DoWork call abstracting the child class from the thread synchronization issues.
        /// </summary>
        /// <param name="sender">The thimer object that is calling the event listener.</param>
        /// <param name="e">The arguments passed by the timer to the method.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {

            //Force other threads to wait until it's finished when calling join.
            this.SyncMutex.Reset();

            //Avoid re-calling the method while it is still operating.
            this.Timer.Stop();

            //Call the worker function.
            this.DoWork(sender, e);

            //Re-Start the timer to execute the worker function endlessly.
            this.Timer.Start();

            //Release threads that might be frozen in join operation.
            this.SyncMutex.Set();

        }

        /// <summary>
        /// This method must be implemented by the child class and must contain the code
        /// to be executed periodically.
        /// </summary>
        protected abstract void DoWork(Object sender, ElapsedEventArgs e);

        /// <summary>
        /// Starts the background task timer that is in charge of executing the DoWork method each
        /// time the interval is elapsed.
        /// </summary>
        public void Start() {
            this.Timer.Start();
        }

        /// <summary>
        /// Stops the background task timer that is in charge of executing the DoWork method each
        /// time the interval is elapsed. If the DoWork method was executing when this method is
        /// called, the caller thread will block waiting the DoWork operation to finish. Later on,
        /// the timer will be stopped. Otherwise, if the DoWork method is not executing when this
        /// method is called, the timer will be stopped without blocking the caller thread.
        /// </summary>
        public void Stop() {
            this.SyncMutex.WaitOne();
            this.SyncMutex.Set();
            this.Timer.Stop();
        }


        /// <summary>
        /// This method can operate in two different ways. If the DoWork method is currently executing, it will
        /// block the caller thread until DoWork finishes. However, if the DoWork method is not being executed,
        /// this method will not block and will immediately return back the control to the caller thread.
        /// </summary>
        public void Join() {
            this.SyncMutex.WaitOne();
            this.SyncMutex.Set();
        }
    }
}

