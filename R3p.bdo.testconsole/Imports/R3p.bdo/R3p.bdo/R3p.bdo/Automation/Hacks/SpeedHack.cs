using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Enums;
using R3p.bdo.Memory;

namespace R3p.bdo.Automation.Hacks
{
    public class SpeedHack
    {
        public static SpeedHack Instance;

        public class SpeedHackActor
        {
            public bool Enabled { get; set; }
            public int Accel { get; set; }
            public int Speed { get; set; }
            public int Turn { get; set; }
            public int Brake { get; set; }
            public int DefaultAccel { get; set; }
            public int DefaultSpeed { get; set; }
            public int DefaultTurn { get; set; }
            public int DefaultBrake { get; set; }

            public SpeedHackActor(bool enabled, int accel, int speed, int turn, int brake, int defaultAccel, int defaultSpeed, int defaultTurn, int defaultBrake)
            {
                Enabled = enabled;
                Accel = accel;
                Speed = speed;
                Turn = turn;
                Brake = brake;
                DefaultAccel = defaultAccel;
                DefaultSpeed = defaultSpeed;
                DefaultTurn = defaultTurn;
                DefaultBrake = defaultBrake;
            }
        }

        public class SpeedHackPlayerActor
        {
            public bool Enabled { get; set; }
            public int Movement { get; set; }
            public int Attack { get; set; }
            public int Cast { get; set; }
            public bool AdvancedMode { get; set; }
            public int Factor { get; set; }

            public SpeedHackPlayerActor(bool enabled, int movement, int attack, int cast, bool advancedMode, int factor)
            {
                Enabled = enabled;
                Movement = movement;
                Attack = attack;
                Cast = cast;
                AdvancedMode = advancedMode;
                Factor = factor;
            }
        }

        public SpeedHackActor Horse { get; set; }
        public SpeedHackActor Ship { get; set; }
        public SpeedHackPlayerActor Player { get; set; }
        public string[] familyNameWhiteList { get; set; }
        public bool GhillieMode { get; set; }
        
        private bool reseted = false;
        public bool hackedAnimation = false;

        public bool globalEnabled = true;

        public SpeedHack()
        {
            Instance = this;
        }
        
        public void Run()
        {
            if (!globalEnabled)
                return;

            if (AutoFish.Instance.isFishing)
                return;

            HackGatheringLevel();

            if (Player.Enabled)
            {
                if (!otherPlayersNearby())
                {
                    reseted = false;
                    HackPlayerSpeeds();

                    if (Player.AdvancedMode)
                    {
                        var animation = Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation;

                        if ((Collection.Actors.Local.PlayerData.isCombatStance &&
                            animation !=CharacterAnimation.Idle &&
                            animation != CharacterAnimation.Running &&
                            animation != CharacterAnimation.CCed) ||
                            (animation == CharacterAnimation.Matchlock) ||
                            (animation == CharacterAnimation.ShipCannon))
                        {
                            HackAnimation();
                        }
                        else
                        {
                            ResetAnimation();
                        }
                    }
                }
                else
                {
                    if (!reseted)
                    {
                        ResetPlayerSpeeds();
                        ResetAnimation();
                    }
                }
            }

            if (Horse.Enabled || Ship.Enabled)
            {
                if (GhillieMode && Collection.Actors.Local.PlayerData.IsCharacterNameTagVisible)
                {
                    RestoreRide();
                    return;
                }

                if (otherPlayersNearby())
                {
                    RestoreRide();
                }
                else
                {
                    HackRide();
                }
            }
        }

        private void HackGatheringLevel()
        {
            if (!Collection.Actors.Local.PlayerData.IsHackedGathering)
            {
                var curLevel = Collection.Actors.Local.PlayerData.CurrentGatheringLevel;

                if (curLevel < 3)
                    return;

                switch (curLevel)
                {
                    case 3:
                        Collection.Actors.Local.PlayerData.SetCurrentGatheringLevel(5);
                        break;

                    case 4:
                        Collection.Actors.Local.PlayerData.SetCurrentGatheringLevel(6);
                        Collection.Actors.Local.PlayerData.SetMaxGatheringLevel(6);
                        break;

                    case 5:
                        Collection.Actors.Local.PlayerData.SetCurrentGatheringLevel(7);
                        Collection.Actors.Local.PlayerData.SetMaxGatheringLevel(7);
                        break;
                }

                Collection.Actors.Local.PlayerData.SetIsHackedGathering();
            }
        }

        private bool otherPlayersNearby()
        {
            var others = Collection.Actors.Global.ActorList.Where(
                x =>
                    x.ActorType == ActorType.ActorType_Player &&
                    x.ActorKey != Collection.Actors.Local.PlayerData.ActorKey &&
                    !familyNameWhiteList.Contains(x.FamilyName.ToLower()));

            return others.Count() != 0;

        }

        private void HackAnimation()
        {
                Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(
                    Convert.ToSingle(Player.Factor));

                hackedAnimation = true;
            
        }

        public void ResetAnimation()
        {
            Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(1.0f);

            hackedAnimation = false;
        }

        private void HackPlayerSpeeds()
        {
            Collection.Actors.Local.PlayerData.setSpeeds(Player.Movement, Player.Attack, Player.Cast, Player.AdvancedMode);
        }

        public void ResetPlayerSpeeds()
        {
            Collection.Actors.Local.PlayerData.setSpeeds(0, 0, 0, Player.AdvancedMode);

            reseted = true;
        }

        private void HackRide()
        {
            if (Collection.Mounts.Rides.CurrentVehicle.ServantType == ServantType.Type_Vehicle && Horse.Enabled && (Collection.Mounts.Rides.CurrentVehicle.Speed < Horse.Speed * 10000 || Collection.Mounts.Rides.CurrentVehicle.Speed > Horse.Speed * 10000))
            {
                //Collection.Mounts.Rides.CurrentVehicle.SetSpeeds(Horse.Accel, Horse.Speed, Horse.Turn, Horse.Brake);
                Collection.Mounts.Rides.CurrentVehicle.SetAccel(Horse.Accel);
                Collection.Mounts.Rides.CurrentVehicle.SetSpeed(Horse.Speed);
                Collection.Mounts.Rides.CurrentVehicle.SetTurn(Horse.Turn);
                Collection.Mounts.Rides.CurrentVehicle.SetBrake(Horse.Brake);

                Log.Post("Hacked Horse Speeds", LogModule.Hack_Speedhack);
            }
            else if(Collection.Mounts.Rides.CurrentVehicle.ServantType == ServantType.Type_Ship && Ship.Enabled && (Collection.Mounts.Rides.CurrentVehicle.Speed < Ship.Speed * 10000 || Collection.Mounts.Rides.CurrentVehicle.Speed > Ship.Speed * 10000))
            {
                //Collection.Mounts.Rides.CurrentVehicle.SetSpeeds(Ship.Accel, Ship.Speed, Ship.Turn, Ship.Brake);
                Collection.Mounts.Rides.CurrentVehicle.SetAccel(Ship.Accel);
                Collection.Mounts.Rides.CurrentVehicle.SetSpeed(Ship.Speed);
                Collection.Mounts.Rides.CurrentVehicle.SetTurn(Ship.Turn);
                Collection.Mounts.Rides.CurrentVehicle.SetBrake(Ship.Brake);

                Log.Post("Hacked Ship Speeds", LogModule.Hack_Speedhack);
            }
        }

        public void SetZeroSpeed()
        {
            if (Collection.Mounts.Rides.CurrentVehicle.ServantType == ServantType.Type_Vehicle && Horse.Enabled && Collection.Mounts.Rides.CurrentVehicle.Speed >= Horse.Speed * 10000)
            {
                //Collection.Mounts.Rides.CurrentVehicle.SetSpeeds(Horse.Accel, 0, Horse.Turn, Horse.Brake);
                Collection.Mounts.Rides.CurrentVehicle.SetSpeed(0);

                Log.Post("Zeroed Horse Speeds", LogModule.Hack_Speedhack);
            }
            else if (Collection.Mounts.Rides.CurrentVehicle.ServantType == ServantType.Type_Ship && Ship.Enabled && Collection.Mounts.Rides.CurrentVehicle.Speed >= Ship.Speed * 10000)
            {
                //Collection.Mounts.Rides.CurrentVehicle.SetSpeeds(Ship.Accel, 0, Ship.Turn, Ship.Brake);
                Collection.Mounts.Rides.CurrentVehicle.SetSpeed(0);

                Log.Post("Zeroed Ship Speeds", LogModule.Hack_Speedhack);
            }
        }

        public void RestoreRide()
        {
            if (Collection.Mounts.Rides.CurrentVehicle.ServantType == ServantType.Type_Vehicle && Horse.Enabled && Collection.Mounts.Rides.CurrentVehicle.Speed >= Horse.Speed*10000)
            {
                //Collection.Mounts.Rides.CurrentVehicle.SetSpeeds(Horse.DefaultAccel, Horse.DefaultSpeed, Horse.DefaultTurn, Horse.DefaultBrake);

                Collection.Mounts.Rides.CurrentVehicle.SetAccel(Horse.DefaultAccel);
                Collection.Mounts.Rides.CurrentVehicle.SetSpeed(Horse.DefaultSpeed);
                Collection.Mounts.Rides.CurrentVehicle.SetTurn(Horse.DefaultTurn);
                Collection.Mounts.Rides.CurrentVehicle.SetBrake(Horse.DefaultBrake);

                Log.Post("Restored Horse Speeds", LogModule.Hack_Speedhack);
            }
            else if (Collection.Mounts.Rides.CurrentVehicle.ServantType == ServantType.Type_Ship && Ship.Enabled && Collection.Mounts.Rides.CurrentVehicle.Speed >= Ship.Speed*10000)
            {
                SetZeroSpeed();
                Thread.Sleep(1500);

                //Collection.Mounts.Rides.CurrentVehicle.SetSpeeds(Ship.DefaultAccel, Ship.DefaultSpeed, Ship.DefaultTurn, Ship.DefaultBrake);

                Collection.Mounts.Rides.CurrentVehicle.SetAccel(Ship.DefaultAccel);
                Collection.Mounts.Rides.CurrentVehicle.SetSpeed(Ship.DefaultSpeed);
                Collection.Mounts.Rides.CurrentVehicle.SetTurn(Ship.DefaultTurn);
                Collection.Mounts.Rides.CurrentVehicle.SetBrake(Ship.DefaultBrake);

                Log.Post("Restored Ship Speeds", LogModule.Hack_Speedhack);
            }
        }
    }
}
