using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Unity.World
{
    /// <summary>
    /// 动作控制器
    /// </summary>
    public class ActionController
    {
        #region public
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="animator"></param>
        public ActionController(Animator animator)
        {
            mLastAction = 0;
            mSetAction = 0;
            mLastNormalizedTime = 0.0f;
            mAnimator = animator;
            mActionListener = new Dictionary<int, List<TimeAction>>();
        }

        public void SetAction(string action)
        {
            mSetAction = GetActionHash(action);
            mAnimator.Play(action, 0, 0.0f);
        }

        public bool IsAction(string action)
        {
            return mSetAction == GetActionHash(action);
        }

        /// <summary>
        /// 完成回调
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="listener"></param>
        public void AddActionCompleteListener(string action, Action<int> listener)
        {
            AddActionCallback(action, 0, 1.0f, listener);
        }

        public void AddActionCallback(string action, int index, float localizedTime, Action<int> callback)
        {
            List<TimeAction> actionList;
            int actionHash = GetActionHash(action);
            if (!mActionListener.TryGetValue(actionHash, out actionList))
            {
                actionList = new List<TimeAction>();
                mActionListener.Add(actionHash, actionList);
            }
            var timeAction = new TimeAction();
            timeAction.action = callback;
            timeAction.localizedTime = localizedTime;
            timeAction.index = index;
            actionList.Add(timeAction);
        }

        public void RemoveActionCallback(string action)
        {
            mActionListener.Remove(GetActionHash(action));
        }

        public void OnUpdate()
        {
            if (mAnimator.IsInTransition(0))
                return;

            var curStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
            int action = curStateInfo.nameHash;
            if (mSetAction == 0)
                mSetAction = action;

            if (!mActionListener.ContainsKey(action))
                return;

            float curNormalizedTime = curStateInfo.normalizedTime;
            float lastTime = mLastNormalizedTime;
            if (mLastAction != action || mLastNormalizedTime >= curNormalizedTime)
            {
                lastTime = 0.0f;
            }

            mLastNormalizedTime = curNormalizedTime;
            mLastAction = action;
            OnAction(lastTime, curNormalizedTime, action);
        }
        #endregion
        #region private
        private int GetActionHash(string action)
        {
            return Animator.StringToHash(action);
        }

        private void OnAction(float lastTime, float curTime, int action)
        {
            var actionList = mActionListener[action];
            foreach (var item in actionList)
            {
                if (item.localizedTime > lastTime && item.localizedTime <= curTime)
                {
                    item.action(item.index);
                }
            }
        }
        private Animator mAnimator;
        private Dictionary<int, List<TimeAction>> mActionListener;
        private int mLastAction;
        private int mSetAction;
        private float mLastNormalizedTime;
        #endregion

        class TimeAction
        {
            public int index;
            public float localizedTime;
            public Action<int> action;
        }

    }
}
