using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using Quintessential.Settings;
using SDL2;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Carms
{

    public class Carms: QuintessentialMod
    {
		public static MethodInfo PublicMethod<T>(string method) => typeof(T).GetMethod(method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		public static MethodInfo PrivateMethod<T>(string method) => typeof(T).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

		public static QuintessentialMod MainClassAsMod;

        public override void Load(){
			Quintessential.Logger.Log("Carms Loaded mrow");
			MainClassAsMod = this;
    	}

    	public override void PostLoad()
		{
			On.SolutionEditorScreen.method_50 += SES_Method_50;
			On.class_153.method_221 += c153_Method_221;

			On.SolutionEditorProgramPanel.method_221 += SolutionEditorProgramPanel_Method_221;

			On.SolutionEditorScreen.method_511 += SES_Method_511;
		}

    	public override void Unload(){
    	}

    	public override void LoadPuzzleContent(){
    		MetricDisplay.LoadPuzzleContent();
    	}


		public static float SES_Method_511(On.SolutionEditorScreen.orig_method_511 orig, SolutionEditorScreen SES_self)
		{
			return orig(SES_self);
		}

		public void SolutionEditorProgramPanel_Method_221(On.SolutionEditorProgramPanel.orig_method_221 orig, SolutionEditorProgramPanel SEPP_self, float param_5658)
		{
			orig(SEPP_self, param_5658);
			MetricDisplay.SEPP_method_221(SEPP_self);
		}

		public void SES_Method_50(On.SolutionEditorScreen.orig_method_50 orig, SolutionEditorScreen SES_self, float param_5703)
		{
			orig(SES_self, param_5703);
		}

		public static void c153_Method_221(On.class_153.orig_method_221 orig, class_153 c153_self, float param_3616)
		{
			orig(c153_self, param_3616);
		}
    }
}