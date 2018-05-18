using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Enums;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class AnimationData : MemoryObject
    {
        public AnimationData(long address)
        {
            Address = address;
        }

        public int AnimationId => ReadInt32(0x00);
        public string AnimationName => GetAnimationName();
        public CharacterAnimation Animation => GetAnimation();

        private string GetAnimationName()
        {
            return Collection.DeezNutz.Base.List.GetEntry(AnimationId).Text;
        }

        private string GetText(int offset)
        {
            return ReadStringASCII(ReadPointer8b(offset));
        }

        private CharacterAnimation GetAnimation()
        {
            if(AnimationName.Contains("RUN"))
                return CharacterAnimation.Running;

            if(AnimationName.Contains("CANNON_SHOOT_SEAT"))
                return CharacterAnimation.ShipCannon;

            switch (AnimationName)
            {
                case "WAIT":
                    return CharacterAnimation.Idle;

                case "WAIT2":
                    return CharacterAnimation.Idle;

                case "BT_WAIT":
                    return CharacterAnimation.Idle;

                case "BT_ARO_WAIT":
                    return CharacterAnimation.Idle;

                case "BT_ARO_WAIT2":
                    return CharacterAnimation.Idle;

                case "FISHING_START":
                    return CharacterAnimation.Fishing_Start1;

                case "FISHING_ING_START":
                    return CharacterAnimation.Fishing_Start2;

                case "FISHING_START_END_Lv0":
                    return CharacterAnimation.Fishing_Start3;

                case "FISHING_WAIT":
                    return CharacterAnimation.Fishing_CatchingWait;

                case "FISHING_HOOK_READY":
                    return CharacterAnimation.Fishing_HookReady;

                case "FISHING_HOOK_ING":
                    return CharacterAnimation.Fishing_Hook;

                case "FISHING_IMPOSSIBLE":
                    return CharacterAnimation.Fishing_Impossible;

                case "DAMAGE_STUN":
                    return CharacterAnimation.CCed;

                case "KNOCKBACK":
                    return CharacterAnimation.CCed;

                case "BT_WAIT_HOLD_ON":
                    return CharacterAnimation.CCed;

                case "KNOCKDOWN":
                    return CharacterAnimation.CCed;

                case "DAMAGE_KNOCKDOWN_HOLDON":
                    return CharacterAnimation.CCed;

                case "DAMAGE_KNOCKDOWN_CANROLL":
                    return CharacterAnimation.CCed;

                case "DAMAGE_KNOCKDOWN_CANROLL_HOLD_ON":
                    return CharacterAnimation.CCed;

                case "DAMAGE_KNOCKDOWN_STANDUP":
                    return CharacterAnimation.CCed;

                case "DAMAGE_RIGID":
                    return CharacterAnimation.CCed;

                case "RIFLE_AIM_START":
                case "RIFLE_AIM_WAIT":
                case "RIFLE_AIM_SHOT2":
                case "RIFLE_AIM_SHOT_FIRE":
                case "RIFLE_AIM_RELOAD":
                    return CharacterAnimation.Matchlock;
                   
                default:
                    return CharacterAnimation.None;
            }
        }
    }
}
