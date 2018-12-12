using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Test : MonoBehaviour
{
// 스레드 동기화 및 일시정지, 재시작
private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
private ManualResetEvent _pauseEvent = new ManualResetEvent(false);

private Thread m_thread;


public void Start()
{
	ThreadTest();
}


public void ThreadTest()
{
m_thread = new Thread(new ThreadStart(threadFunc));
m_thread.Name = "test";
m_thread.IsBackground = true;
m_thread.Start();
}


private void threadFunc()
{
int i = 0;

while(_pauseEvent.WaitOne())
{
	// System.Diagnostics.Trace.WriteLine(i.ToString());
	Debug.Log(" i = " + i);
	i++;
	Thread.Sleep(1000);

	for (int j=0; j<100; j++) {
		Debug.Log(" i + j = " + i + " + " + j );
		Thread.Sleep(500);
	}

}
}


// --------------------------------------------------------------------------------
// 스레드 일시정지
// --------------------------------------------------------------------------------
public void pause()
{
_pauseEvent.Reset();
}//end pause()

// --------------------------------------------------------------------------------
// 스레드 재시작
// --------------------------------------------------------------------------------
public void resume()
{
_pauseEvent.Set();
}//end resume()

// --------------------------------------------------------------------------------
// 스레드 종료 준비, 동기화 이벤트 처리
// --------------------------------------------------------------------------------
public void readyEndThread()
{
_shutdownEvent.Set();
_pauseEvent.Set();
}//end endThread()
}





