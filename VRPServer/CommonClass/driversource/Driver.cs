using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommonClass.driversource
{
    public class Resistance
    {
        public int defensiveOfElectic { get; protected set; }
        public int defensiveOfWater { get; protected set; }
        public int defensiveOfFire { get; protected set; }
        public int defensiveOfLose { get; protected set; }
        public int defensiveOfConfuse { get; protected set; }
        public int defensiveOfAmbush { get; protected set; }
        public int defensiveOfPhysics { get; protected set; }
    }
    public abstract class DriverSkill : Resistance
    {
        public int Index { get; set; }
    }

    public enum Sex
    {
        man, woman
    }
    public enum Race
    {
        devil,
        people,
        immortal
    }
    /// <summary>
    /// 抵抗力
    /// </summary>

    public class Driver : DriverSkill
    {
        public Driver(Sex s, Race r, string n, int index)
        {
            this.sex = s;
            this.race = r;
            this.Name = n;
            this.Index = index;
            skillInitialize();
            defensiveInitialize();
        }
        public Driver(Sex s, Race r, string n)
        {
            this.sex = s;
            this.race = r;
            this.Name = n;
            this.Index = -1;
            skillInitialize();
        }

        const int Electic = 2;
        const int Water = 3;
        const int Fire = 5;
        const int Confuse = 7;
        /// <summary>
        /// 更多伤害，代表潜伏
        /// </summary>
        const int Ambush = 11;
        /// <summary>
        /// 迷失
        /// </summary>
        const int Lose = 13;


        int _volumeGrouth = 100;
        int _bussinessGrouth = 100;
        int _speedGrouth = 100;
        public void GrouthSet(int volumeGrouth, int bussinessGrouth, int speedGrouth)
        {
            this._volumeGrouth = volumeGrouth;
            this._bussinessGrouth = bussinessGrouth;
            this._speedGrouth = speedGrouth;
        }

        public long improveBusiness(int v)
        {
            return this._bussinessGrouth * v / 100;
        }
        public long improveVolume(int v)
        {
            return this._volumeGrouth * v / 100;
        }
        public int improveSpeed(int v)
        {
            return this._speedGrouth * v / 100;
        }
        const int Physics = 17;

        const int modBase = 19;

        public void DefensiveInitialize(int electric, int water, int fire, int confuse, int moreHurt, int lose, int physics)
        {

            this.defensiveOfElectic = electric;
            this.defensiveOfWater = water;
            this.defensiveOfFire = fire;

            this.defensiveOfConfuse = confuse;
            this.defensiveOfAmbush = moreHurt;
            this.defensiveOfLose = lose;


            this.defensiveOfPhysics = physics;
        }
        private void defensiveInitialize()
        {
            //  switch(this.race)

            switch (this.race)
            {
                case Race.devil:
                    {
                        this.defensiveOfElectic = 20 + 1 * delaWith(Electic);
                        this.defensiveOfWater = 20 + 1 * delaWith(Water);
                        this.defensiveOfFire = 20 + 1 * delaWith(Fire);

                        this.defensiveOfConfuse = 20 + 1 * delaWith(Confuse);
                        this.defensiveOfAmbush = 20 + 1 * delaWith(Ambush);
                        this.defensiveOfLose = 20 + 1 * delaWith(Lose);

                        this.defensiveOfPhysics = 20 + 2 * delaWith(Physics);
                    }; break;
                case Race.people:
                    {
                        this.defensiveOfElectic = 0 + 1 * delaWith(Electic);
                        this.defensiveOfWater = 0 + 1 * delaWith(Water);
                        this.defensiveOfFire = 0 + 1 * delaWith(Fire);

                        this.defensiveOfConfuse = 40 + 2 * delaWith(Confuse);
                        this.defensiveOfAmbush = 40 + 2 * delaWith(Ambush);
                        this.defensiveOfLose = 40 + 2 * delaWith(Lose);

                        this.defensiveOfPhysics = 0 + 1 * delaWith(Physics);
                    }; break;
                case Race.immortal:
                    {
                        this.defensiveOfElectic = 40 + 2 * delaWith(Electic);
                        this.defensiveOfWater = 40 + 2 * delaWith(Water);
                        this.defensiveOfFire = 40 + 2 * delaWith(Fire);

                        this.defensiveOfConfuse = 0 + 1 * delaWith(Confuse);
                        this.defensiveOfAmbush = 0 + 1 * delaWith(Ambush);
                        this.defensiveOfLose = 0 + 1 * delaWith(Lose);



                        this.defensiveOfPhysics = 0 + 1 * delaWith(Physics);
                    }; break;

            }

        }
        int delaWith(int a)
        {
            var b = this.Index;
            var t = ex_reverse((a + b) % modBase, modBase) + b;
            var t2 = t % modBase;
            var t3 = (t2 * t2) % modBase;
            var t4 = (ex_reverse(this.skill1.Index, modBase) * this.skill2.Index + this.skill2.Index) % modBase;
            var t5 = (ex_reverse(t4, modBase) * this.skill1.Index + this.skill1.Index) % modBase;
            for (var i = 0; i < t5; i++)
            {
                t3 = ((t3 * t3) % modBase + ((a + b == 19) ? (i / 2 + 1) : ex_reverse((a + b) % modBase, modBase))) % modBase;

            }
            return t3;
        }
        private void skillInitialize()
        {
            switch (this.race)
            {
                case Race.devil:
                    {
                        this.skill1 = new Skill(SkillEnum.Attack);
                        switch (this.sex)
                        {
                            case Sex.man:
                                {
                                    this.skill2 = new Skill(SkillEnum.Speed);
                                }; break;
                            case Sex.woman:
                                {
                                    this.skill2 = new Skill(SkillEnum.Defense);
                                }; break;
                        }
                    }; break;
                case Race.people:
                    {
                        this.skill1 = new Skill(SkillEnum.Ambush);
                        switch (this.sex)
                        {
                            case Sex.man:
                                {
                                    this.skill2 = new Skill(SkillEnum.Confusion);
                                }; break;
                            case Sex.woman:
                                {
                                    this.skill2 = new Skill(SkillEnum.Lose);
                                }; break;
                        }
                    }; break;
                case Race.immortal:
                    {
                        this.skill1 = new Skill(SkillEnum.Water);
                        switch (this.sex)
                        {
                            case Sex.man:
                                {
                                    this.skill2 = new Skill(SkillEnum.Electic);
                                }; break;
                            case Sex.woman:
                                {
                                    this.skill2 = new Skill(SkillEnum.Fire);
                                }; break;
                        }
                    }; break;
            }



        }

        public Sex sex { get; private set; }
        public Race race { get; private set; }
        public string Name { get; private set; }




        public string defensiveString
        {
            get
            {
                return $"{this.Name},{this.defensiveOfElectic.ToString("00")},{this.defensiveOfWater.ToString("00")},{this.defensiveOfFire.ToString("00")},{this.defensiveOfLose.ToString("00")},{this.defensiveOfConfuse.ToString("00")},{this.defensiveOfAmbush.ToString("00")},{this.defensiveOfPhysics.ToString("00")}";
            }
        }
        public Skill skill1 { get; private set; }
        public Skill skill2 { get; private set; }

        public static int ex_gcd(int a, int b, out int x, out int y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            int r = ex_gcd(b, a % b, out x, out y);
            int t = x;
            x = y;
            y = t - a / b * y;
            return r;
        }

        public static int ex_reverse(int a, int b)
        {
            int d, x, y;
            d = ex_gcd(a, b, out x, out y);
            if (d == 1)
            {
                return (x % b + b) % b;
            }
            else
            {
                return -1;
            }
        }


    }

    public enum SkillEnum
    {
        Electic,
        Water,
        Fire,
        Confusion,
        Ambush,
        Lose,
        Speed,
        Attack,
        Defense

    }
    public class Skill : DriverSkill
    {
        public SkillEnum skillEnum { get; private set; }
        public Skill(SkillEnum se)
        {
            this.skillEnum = se;
            this.Index = (int)se * 2 + 1;

            switch (this.skillEnum)
            {
                case SkillEnum.Electic:
                    {
                        this.skillName = "雷击";
                    }; break;
                case SkillEnum.Water:
                    {
                        this.skillName = "水淹";
                    }; break;
                case SkillEnum.Fire:
                    {
                        this.skillName = "火烧";
                    }; break;
                case SkillEnum.Confusion:
                    {
                        this.skillName = "混乱";
                    }; break;
                case SkillEnum.Ambush:
                    {
                        this.skillName = "潜伏";
                    }; break;
                case SkillEnum.Lose:
                    {
                        this.skillName = "迷失";
                    }; break;
                case SkillEnum.Speed:
                    {
                        this.skillName = "提速";
                    }; break;
                case SkillEnum.Attack:
                    {
                        this.skillName = "红牛";
                    }; break;
                case SkillEnum.Defense:
                    {
                        this.skillName = "加防";
                    }; break;
                default:
                    {
                        this.skillName = "";
                    }; break;
            }
        }
        public string skillName { get; private set; }

    }
}
