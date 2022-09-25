using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

public class JobSystemTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
        Combination combinationJob = new Combination();
        combinationJob.Init(5, 15);
        // combinationJob.result = result;

        JobHandle jobHandle = combinationJob.Schedule();

        jobHandle.Complete();
        int c = combinationJob.result[0];
        // result.Dispose();
        combinationJob.Dispose();
        Debug.Log($"A + B = {c}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [BurstCompile]
    public struct Combination : IJob
    {
        int a;
        int b;
        public NativeArray<int> result;
        public void Init(int a, int b)
        {
            this.a = a;
            this.b = b;
            result = new NativeArray<int>(1, Allocator.TempJob);
        }
        public void Execute()
        {
            result[0] = a + b;
        }

        public void Dispose()
        {
            result.Dispose();
        }
    }
}
