using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using SDL2;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Carms
{
	using Texture = class_256;
	using PartType = class_139;

    public static class MetricDisplay
    {
    	//data structs, enums, variables
    	private static Texture metric_overlay;

    	public static Vector2 textureDimensions(Texture texture) => texture.field_2056.ToVector2();

		private enum resource : byte
		{
			create,
			COUNT,
		}

		public static PartType MechanismArm1() => class_191.field_1764;
		public static PartType MechanismArm2() => class_191.field_1765;
		public static PartType MechanismArm3() => class_191.field_1766;
		public static PartType MechanismArm6() => class_191.field_1767;
		public static PartType MechanismPiston() => class_191.field_1768;
		public static PartType IOOutputStandard() => class_191.field_1761;

		public static bool PartIsArm(Part part) {
			return  part.method_1159() == MechanismArm1() ||
					part.method_1159() == MechanismArm2() ||
					part.method_1159() == MechanismArm3() ||
					part.method_1159() == MechanismArm6() ||
					part.method_1159() == MechanismPiston();
		}

		public static HexIndex getPartOrigin(Part part) => part.method_1161();

        private static void display_metric(string name, string value, Vector2 position, float offset = 0f)
		{
			//copied from method_2086()
			Texture score_card = class_238.field_1989.field_99.field_706.field_746;
			Texture text_gradient = class_238.field_1989.field_99.field_706.field_751;

			position += new Vector2(Input.ScreenSize().X - 364.0f, 268f); // base position

			var font = class_238.field_1990.field_2142;
			class_135.method_272(score_card, position);
			class_135.method_290(name.method_441(), position + new Vector2(offset - 46f, 6f), font, Color.White, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, -2, Color.Black, text_gradient, int.MaxValue, false, true);
			class_135.method_290(value.method_441(), position + new Vector2(30f, 6f), font, Color.FromHex(3483687), (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, new Color(), (class_256)null, int.MaxValue, false, true);
		}

    	//---------------------------------------------------//
    	//internal main methods

    	//---------------------------------------------------//
    	public static void SEPP_method_221(SolutionEditorProgramPanel SEPPSelf)
    	{
    		var SES = new DynamicData(SEPPSelf).Get<SolutionEditorScreen>("field_2007");
			bool simRunning = (SES.method_503() != enum_128.Stopped);

			Vector2 metric_overlay_position = new Vector2(Input.ScreenSize().X - textureDimensions(metric_overlay).X - 300f, 296f - 32f);

    		//-------------- compute metrics --------------//
    		var maybeSim = Sim.method_1824(SES);
    		bool validProgram = maybeSim.method_1085();

    		// base metrics
    		int cycles = SES.field_4017.method_1090(SES.method_2127());

    		// program metrics
    		int arms = 0;
			int DOut = 0;
    		if (validProgram)
    		{
    			CompiledProgramGrid compiledProgramGrid = maybeSim.method_1087().method_1820();
    			var programGridDict = new DynamicData(compiledProgramGrid).Get<Dictionary<Part, CompiledProgram>>("field_2368");
    			arms = 0;
				List<HexIndex> outputIndexes = new List<HexIndex>{};
    			foreach (var ENTRY in programGridDict)
    			{
					if(PartIsArm(ENTRY.Key)) arms++;
					else if (ENTRY.Key.method_1159() == IOOutputStandard())
					{
						HexIndex thisIndex = getPartOrigin(ENTRY.Key);
						foreach (var hexI in outputIndexes)
						{
							DOut = Math.Max(DOut,HexIndex.Distance(hexI,thisIndex));
						}
						outputIndexes.Add(thisIndex);
					}
    			}
    		}

    		float xpos;

			// class_135.method_272(metric_overlay, metric_overlay_position);

    		xpos = -670f;
    		display_metric(class_134.method_253("Carms", string.Empty).method_1060() + ":", simRunning ? (cycles*arms).method_453() : "----", new Vector2(xpos, 0));
			xpos = -820f;
    		display_metric(class_134.method_253("MaxOD", string.Empty).method_1060() + ":", (DOut>0) ? (DOut).method_453() : "----", new Vector2(xpos, 0));
    	}

    	public static void LoadPuzzleContent()
    	{
    		//load texture
    		string path = "carms/textures/";
    		metric_overlay = class_235.method_615(path + "metric_overlay");
    	}

    	//---------------------------------------------------//
    }
}