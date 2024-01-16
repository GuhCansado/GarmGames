namespace NinjutsuGames.StateMachine.Editor
{
    using System.Collections;
    using UnityEditor;

    public class EditorCoroutine
    {
        private readonly IEnumerator _routine;

        private EditorCoroutine(IEnumerator routine)
        {
            _routine = routine;
        }

        public static EditorCoroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = new EditorCoroutine(routine);
            EditorApplication.update += coroutine.Update;
            return coroutine;
        }

        private void Stop()
        {
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            if (!_routine.MoveNext())
            {
                Stop();
            }
        }
    }

}