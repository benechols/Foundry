﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Foundry
{
    public enum Types
    {
        [Description("he_learner")]
        HELearner,
        [Description("he_admin")]
        HEAdmin,
        [Description("fac_staff_learner")]
        FacStaffLearner,
        [Description("fac_staff_admin")]
        FacStaffAdmin,
        [Description("cc_learner")]
        CCLearner,
        [Description("cc_admin")]
        CCAdmin,
        [Description("next_learner")]
        AdultFinancialLearner,
        [Description("at_work_manager")]
        AdultFinancialManager,
        [Description("event_volunteer")]
        EventVolunteer,
        [Description("event_manager")]
        EventManager
    };
    public enum Roles
    {
        [Description("undergrad")]
        Undergraduate,
        [Description("graduate")]
        Graduate,
        [Description("non_traditional")]
        NonTraditional,
        [Description("greek")]
        Greek,
        [Description("primary")]
        Primary,
        [Description("supervisor")]
        Supervisor,
        [Description("non_supervisor")]
        NonSupervisor,
        [Description("default")]
        Default
    };
    public class UserType
    {
        public Types Type { get; set; }
        public Roles Role { get; set; }

        public UserType(Types MyType, Roles MyRole)
        {
            this.Type = MyType;
            this.Role = MyRole;
        }

        internal static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }

        internal static Types StringToType(string value)
        {
            switch (value)
            {
                case "Higher Education Student":
                    return Types.HELearner;
                case "High Education Training Admin":
                    return Types.HEAdmin;
                case "Faculty/Staff Admin":
                    return Types.FacStaffAdmin;
                case "Faculty/Staff Learner":
                    return Types.FacStaffLearner;
                case "Employee Learner":
                    return Types.CCLearner;
                case "Employee Training Admin":
                    return Types.CCAdmin;
                case "Events Admin":
                    return Types.EventManager;
                case "Events Volunteer":
                    return Types.EventVolunteer;
                // TODO: Add Financial Learner and Manager
                default:
                    return Types.HELearner; // TODO: return something for default
            }
        }

        internal static Roles StringToRole(string value)
        {
            switch (value)
            {
                case "Undergrad":
                    return Roles.Undergraduate;
                case "Primary":
                    return Roles.Primary;
                case "Graduate":
                    return Roles.Graduate;
                case "Greek":
                    return Roles.Greek;
                case "Non-supervisor": // Check to make sure the s is not capitalized
                    return Roles.NonSupervisor;
                case "Non-Traditional":
                    return Roles.NonTraditional;
                case "Supervisor":
                    return Roles.Supervisor;
                case "Default":
                    return Roles.Default;
                default:
                    return Roles.Default; // TODO: return something for default
            }
        }
    }
}
