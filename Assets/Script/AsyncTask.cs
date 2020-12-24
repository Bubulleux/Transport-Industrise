using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class AsyncTask 
{
    public static async Task DelayIf(int curValue, int when, int timeToWait)
    {
        if (curValue % when == 0)
        {
            await Task.Delay(timeToWait);
        }
    }
}
