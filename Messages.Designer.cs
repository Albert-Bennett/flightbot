﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlightBot {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FlightBot.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thats perfect! Can you tell me what your destination is?.
        /// </summary>
        public static string AIRPORT_CONFIRMED {
            get {
                return ResourceManager.GetString("AIRPORT_CONFIRMED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thats fine, please let me know which airport are you flying from..
        /// </summary>
        public static string AIRPORT_NOT_CONFIRMED {
            get {
                return ResourceManager.GetString("AIRPORT_NOT_CONFIRMED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hmm... I can&apos;t find that particular airport do you have the airport code for it or do you want me to try and find another airport.
        /// </summary>
        public static string AIRPORT_NOT_FOUND {
            get {
                return ResourceManager.GetString("AIRPORT_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We are off to a flying start! Here are some flights that I have found to {DESTINATION} on the {DATE}..
        /// </summary>
        public static string DATE_CONFIRMED {
            get {
                return ResourceManager.GetString("DATE_CONFIRMED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I&apos;ve checked for flights from {AIRPORT} to {DESTINATION} and it looks like that airport isn&apos;t servicing that destination right now. {AIRPORTS_NEARBY}.
        /// </summary>
        public static string DESTINATION_NOT_AVAILIBLE {
            get {
                return ResourceManager.GetString("DESTINATION_NOT_AVAILIBLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Super! What date are you planning on flying to {DESTINATION}?.
        /// </summary>
        public static string DESTINATON_CONFIRMED {
            get {
                return ResourceManager.GetString("DESTINATON_CONFIRMED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I&apos;ve found these airports near you. Which one are you traveling from?.
        /// </summary>
        public static string FOUND_MANY_AIRPORTS {
            get {
                return ResourceManager.GetString("FOUND_MANY_AIRPORTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hmm... I can&apos;t find any nearby airports. Can you tell me where you will be flying from?.
        /// </summary>
        public static string FOUND_NO_AIRPORTS {
            get {
                return ResourceManager.GetString("FOUND_NO_AIRPORTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hmm... it looks like the closest airport to you is:.
        /// </summary>
        public static string FOUND_ONE_AIRPORT {
            get {
                return ResourceManager.GetString("FOUND_ONE_AIRPORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unfortunatly, I can&apos;t find any flights from {AIRPORT} to {DESTINATION} on {DATE}. Please enter a new date and, I&apos;ll search again or you can tell me in plain English where you&apos;d like to go and the airport that you are traveling from..
        /// </summary>
        public static string NO_FLIGHTS_FOUND {
            get {
                return ResourceManager.GetString("NO_FLIGHTS_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please tell me of another airport that you would like me to search for availible flight from?.
        /// </summary>
        public static string NO_OTHER_AIRPORTS_NEARBY {
            get {
                return ResourceManager.GetString("NO_OTHER_AIRPORTS_NEARBY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to None of these options.
        /// </summary>
        public static string NONE_OF_THESE {
            get {
                return ResourceManager.GetString("NONE_OF_THESE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I had found these other airports near you. Do you want me to check them for flights going to {DESTINATION}?.
        /// </summary>
        public static string SOME_OTHER_AIRPORTS_NEARBY {
            get {
                return ResourceManager.GetString("SOME_OTHER_AIRPORTS_NEARBY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi there, I&apos;d like to help you find flights. {AIRPORT_RESPONSE}.
        /// </summary>
        public static string WELCOME_MESSAGE {
            get {
                return ResourceManager.GetString("WELCOME_MESSAGE", resourceCulture);
            }
        }
    }
}
