extern alias StatsLib;
using RecalculateStatsAPI = StatsLib::R2API.RecalculateStatsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using EntityStates.Huntress;
using JetBrains.Annotations;
using KinematicCharacterController;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using Rewired;
using RoR2;
using RoR2.Achievements;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.UI;


using orig_Start = On.RoR2.SceneDirector.orig_Start;
using orig_OnInventoryChanged = On.RoR2.CharacterMaster.orig_OnInventoryChanged;
using orig_RespawnExtraLife = On.RoR2.CharacterMaster.orig_RespawnExtraLife;

namespace Auriel;









[BepInPlugin("com.Dragonyck.Auriel", "Auriel", "1.7.2")]
public class Auriel : BaseUnityPlugin
{
	public const string MODUID = "com.Dragonyck.Auriel";

	public static GameObject characterPrefab;

	public static GameObject crosshairPrefab;

	public GameObject characterDisplay;

	public GameObject doppelganger;

	public static ConfigEntry<float> crosshairSize;

	public static ConfigEntry<KeyCode> EnergyKeyToggle;

	public static BuffDef crownBuff;

	public static readonly Color characterColor = new Color(1f, 0.8f, 0.12f);

	public static AssetBundle MainAssetBundle = null;

	public static SkillDef secondarySkillDef;

	public static SkillDef utilitySkillDef;

	public static SkillDef specialSkillDef;

	public static SkillDef secondaryEmpSkillDef;

	public static SkillDef utilityEmpSkillDef;

	public static SkillDef specialEmpSkillDef;

	private Transform crownOn;

	public static readonly uint BarMax = 508719834u;

	public static readonly uint HealField = 651778837u;

	public static readonly uint M1 = 1685527111u;

	public static readonly uint HealFieldMax = 2064178461u;

	public static readonly uint Utility = 2378126911u;

	public static readonly uint res_Source = 2781556679u;

	public static readonly uint Special = 3064974266u;

	public static readonly uint Switch = 3202316517u;

	public static readonly uint res_Target = 3939982387u;

	public static readonly uint Laser = 3982605422u;

	public static readonly uint LaserMax = 4269721684u;

	private void Awake()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00aa: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00de: Expected O, but got Unknown
		if ((Object)(object)MainAssetBundle == (Object)null)
		{
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Auriel.AssetBundle.auriel");
			MainAssetBundle = AssetBundle.LoadFromStream(stream);
		}
		using (Stream stream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Auriel.Auriel.bnk"))
		{
			byte[] array = new byte[stream2.Length];
			_ = stream2.Read(array, 0, array.Length);
			SoundBanks.Add(array);
		}
		crosshairSize = ((BaseUnityPlugin)this).Config.Bind<float>(new ConfigDefinition("Crosshair Size", "Size"), 50f, new ConfigDescription("", (AcceptableValueBase)null, Array.Empty<object>()));
		EnergyKeyToggle = ((BaseUnityPlugin)this).Config.Bind<KeyCode>(new ConfigDefinition("Energy Skill Toggle", "Key"), (KeyCode)308, new ConfigDescription("", (AcceptableValueBase)null, Array.Empty<object>()));
		Languages.AddLanguageSupport();
		Achievements.RegisterUnlockables();
		Prefabs.CreatePrefabs();
		CreatePrefab();
		RegisterStates();
		RegisterCharacter();
		CreateDoppelganger();
		Hooks();
		VFX.RegisterGenericEffects();
	}

	internal static T Load<T>(string path)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Addressables.LoadAssetAsync<T>((object)path).WaitForCompletion();
	}

	private void Hooks()
{
    //IL_0008 ...
    RecalculateStatsAPI.GetStatCoefficients += new StatHookEventHandler(RecalculateStatsAPI_GetStatCoefficients);
}

private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
{
    int buffCount = sender.GetBuffCount(crownBuff);
    if (buffCount > 0)
    {
        args.attackSpeedMultAdd += 0.45f * (float)buffCount;
        args.moveSpeedMultAdd += 0.45f * (float)buffCount;
    }
}

	private static GameObject CreateModel(GameObject main)
	{
		Object.Destroy((Object)(object)((Component)main.transform.Find("ModelBase")).gameObject);
		Object.Destroy((Object)(object)((Component)main.transform.Find("CameraPivot")).gameObject);
		Object.Destroy((Object)(object)((Component)main.transform.Find("AimOrigin")).gameObject);
		return MainAssetBundle.LoadAsset<GameObject>("Aurielmdl");
	}

	internal static void CreatePrefab()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Expected O, but got Unknown
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb0: Expected O, but got Unknown
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d98: Expected O, but got Unknown
		//IL_0d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1028: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Expected O, but got Unknown
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_106e: Unknown result type (might be due to invalid IL or missing references)
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1201: Unknown result type (might be due to invalid IL or missing references)
		//IL_1203: Unknown result type (might be due to invalid IL or missing references)
		//IL_120c: Unknown result type (might be due to invalid IL or missing references)
		//IL_123c: Unknown result type (might be due to invalid IL or missing references)
		//IL_123e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1283: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c6: Expected O, but got Unknown
		//IL_12cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1305: Unknown result type (might be due to invalid IL or missing references)
		//IL_140b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Unknown result type (might be due to invalid IL or missing references)
		//IL_151c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1610: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f7: Expected O, but got Unknown
		//IL_172b: Unknown result type (might be due to invalid IL or missing references)
		//IL_173d: Unknown result type (might be due to invalid IL or missing references)
		//IL_175e: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b7: Expected O, but got Unknown
		//IL_17eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_181e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1870: Unknown result type (might be due to invalid IL or missing references)
		//IL_1877: Expected O, but got Unknown
		//IL_18ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Unknown result type (might be due to invalid IL or missing references)
		characterPrefab = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/Commando/CommandoBody.prefab"), "AurielBody", true);
		characterPrefab.AddComponent<EnergyController>();
		GameObject val = CreateModel(characterPrefab);
		GameObject val2 = new GameObject("ModelBase");
		val2.transform.parent = characterPrefab.transform;
		val2.transform.localPosition = new Vector3(0f, -0.81f, 0f);
		val2.transform.localRotation = Quaternion.identity;
		val2.transform.localScale = new Vector3(1f, 1f, 1f);
		GameObject val3 = new GameObject("CameraPivot");
		val3.transform.parent = val2.transform;
		val3.transform.localPosition = new Vector3(0f, 2.8f, 0f);
		val3.transform.localRotation = Quaternion.identity;
		val3.transform.localScale = Vector3.one;
		GameObject val4 = new GameObject("AimOrigin");
		val4.transform.parent = val2.transform;
		val4.transform.localPosition = new Vector3(0f, 2.6f, 0f);
		val4.transform.localRotation = Quaternion.identity;
		val4.transform.localScale = Vector3.one;
		Transform transform = val.transform;
		transform.parent = val2.transform;
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.localRotation = Quaternion.identity;
		CharacterDirection component = characterPrefab.GetComponent<CharacterDirection>();
		component.moveVector = Vector3.zero;
		component.targetTransform = val2.transform;
		component.overrideAnimatorForwardTransform = null;
		component.rootMotionAccumulator = null;
		component.modelAnimator = val.GetComponentInChildren<Animator>();
		component.driveFromRootRotation = false;
		component.turnSpeed = 720f;
		CharacterBody component2 = characterPrefab.GetComponent<CharacterBody>();
		((Object)component2).name = "AurielBody";
		component2.baseNameToken = "AURIEL_NAME";
		component2.subtitleNameToken = "AURIEL_SUBTITLE";
		component2.bodyFlags = (BodyFlags)16;
		component2.rootMotionInMainState = false;
		component2.mainRootSpeed = 0f;
		component2.baseMaxHealth = 110f;
		component2.levelMaxHealth = 33f;
		component2.baseRegen = 1f;
		component2.levelRegen = 0.25f;
		component2.baseMaxShield = 0f;
		component2.levelMaxShield = 0f;
		component2.baseMoveSpeed = 7f;
		component2.levelMoveSpeed = 0f;
		component2.baseAcceleration = 80f;
		component2.baseJumpPower = 15f;
		component2.levelJumpPower = 0f;
		component2.baseDamage = 12f;
		component2.levelDamage = 2.4f;
		component2.baseAttackSpeed = 1f;
		component2.levelAttackSpeed = 0f;
		component2.baseCrit = 1f;
		component2.levelCrit = 0f;
		component2.baseArmor = 0f;
		component2.levelArmor = 0f;
		component2.baseJumpCount = 1;
		component2.sprintingSpeedMultiplier = 1.45f;
		component2.wasLucky = false;
		component2._defaultCrosshairPrefab = Prefabs.Crosshair;
		component2.hideCrosshair = false;
		component2.aimOriginTransform = val4.transform;
		component2.hullClassification = (HullClassification)0;
		component2.portraitIcon = (Texture)(object)MainAssetBundle.LoadAsset<Sprite>("portrait").texture;
		component2.isChampion = false;
		component2.currentVehicle = null;
		component2.skinIndex = 0u;
		component2.preferredPodPrefab = null;
		EntityStateMachine component3 = characterPrefab.GetComponent<EntityStateMachine>();
		component3.mainStateType = new SerializableEntityStateType(typeof(AurielCharacterMain));
		CharacterMotor component4 = characterPrefab.GetComponent<CharacterMotor>();
		component4.walkSpeedPenaltyCoefficient = 1f;
		component4.characterDirection = component;
		component4.muteWalkMotion = false;
		component4.mass = 100f;
		component4.airControl = 0.25f;
		component4.disableAirControlUntilCollision = false;
		component4.generateParametersOnAwake = true;
		InputBankTest component5 = characterPrefab.GetComponent<InputBankTest>();
		component5.moveVector = Vector3.zero;
		ModelLocator component6 = characterPrefab.GetComponent<ModelLocator>();
		component6.modelTransform = transform;
		component6.modelBaseTransform = val2.transform;
		component6.dontReleaseModelOnDeath = false;
		component6.autoUpdateModelTransform = true;
		component6.dontDetatchFromParent = false;
		component6.noCorpse = false;
		component6.normalizeToFloor = false;
		component6.preserveModel = false;
		ChildLocator component7 = val.GetComponent<ChildLocator>();
		CharacterModel val5 = val.AddComponent<CharacterModel>();
		val5.body = component2;
		val5.baseRendererInfos = (RendererInfo[])(object)new RendererInfo[4]
		{
			new RendererInfo
			{
				defaultMaterial = ((Renderer)val.GetComponentInChildren<SkinnedMeshRenderer>()).material,
				renderer = (Renderer)(object)val.GetComponentInChildren<SkinnedMeshRenderer>(),
				defaultShadowCastingMode = (ShadowCastingMode)1,
				ignoreOverlays = false
			},
			new RendererInfo
			{
				renderer = (Renderer)(object)((Component)component7.FindChild("Wings")).GetComponentInChildren<SkinnedMeshRenderer>(),
				defaultMaterial = ((Renderer)((Component)component7.FindChild("Wings")).GetComponentInChildren<SkinnedMeshRenderer>()).material,
				defaultShadowCastingMode = (ShadowCastingMode)1,
				ignoreOverlays = false
			},
			new RendererInfo
			{
				renderer = (Renderer)(object)((Component)component7.FindChild("WingsAnimated")).GetComponentInChildren<SkinnedMeshRenderer>(),
				defaultMaterial = ((Renderer)((Component)component7.FindChild("WingsAnimated")).GetComponentInChildren<SkinnedMeshRenderer>()).material,
				defaultShadowCastingMode = (ShadowCastingMode)1,
				ignoreOverlays = false
			},
			new RendererInfo
			{
				renderer = (Renderer)(object)((Component)component7.FindChild("Crown")).GetComponentInChildren<SkinnedMeshRenderer>(),
				defaultMaterial = ((Renderer)((Component)component7.FindChild("Crown")).GetComponentInChildren<SkinnedMeshRenderer>()).material,
				defaultShadowCastingMode = (ShadowCastingMode)1,
				ignoreOverlays = false
			}
		};
		val5.autoPopulateLightInfos = true;
		val5.invisibilityCount = 0;
		val5.temporaryOverlays = new List<TemporaryOverlayInstance>();
		Reflection.SetFieldValue<SkinnedMeshRenderer>((object)val5, "mainSkinnedMeshRenderer", ((Component)val5.baseRendererInfos[0].renderer).gameObject.GetComponent<SkinnedMeshRenderer>());
		GameObject gameObject = ((Component)characterPrefab.GetComponentInChildren<ModelLocator>().modelTransform).gameObject;
		CharacterModel component8 = gameObject.GetComponent<CharacterModel>();
		ModelSkinController val6 = gameObject.AddComponent<ModelSkinController>();
		ChildLocator component9 = gameObject.GetComponent<ChildLocator>();
		SkinnedMeshRenderer fieldValue = Reflection.GetFieldValue<SkinnedMeshRenderer>((object)component8, "mainSkinnedMeshRenderer");
		LanguageAPI.Add("AURIELBODY_DEFAULT_SKIN_NAME", "Default");
		LanguageAPI.Add("AURIELBODY_ARCHANGEL_SKIN_NAME", "Archangel");
		LanguageAPI.Add("AURIELBODY_SAKURA_SKIN_NAME", "Sakura");
		LanguageAPI.Add("AURIELBODY_SPIRIT_SKIN_NAME", "Spirit Healer");
		LanguageAPI.Add("AURIELBODY_DEMONIC_SKIN_NAME", "Demonic");
		Material val7 = Load<Material>("RoR2/Base/Commando/matCommandoDualies.mat");
		SkinDefInfo val8 = default(SkinDefInfo);
		val8.BaseSkins = Array.Empty<SkinDef>();
		val8.MinionSkinReplacements = (MinionSkinReplacement[])(object)new MinionSkinReplacement[0];
		val8.ProjectileGhostReplacements = (ProjectileGhostReplacement[])(object)new ProjectileGhostReplacement[0];
		((Component)component9.FindChild("Crown")).gameObject.SetActive(false);
		val8.GameObjectActivations = (GameObjectActivation[])(object)new GameObjectActivation[0];
		val8.Icon = MainAssetBundle.LoadAsset<Sprite>("portraitskin");
		RendererInfo[] baseRendererInfos = val5.baseRendererInfos;
		val8.MeshReplacements = (MeshReplacement[])(object)new MeshReplacement[4]
		{
			new MeshReplacement
			{
				renderer = (Renderer)(object)fieldValue,
				mesh = fieldValue.sharedMesh
			},
			new MeshReplacement
			{
				renderer = baseRendererInfos[1].renderer,
				mesh = ((Component)component9.FindChild("Wings")).GetComponent<SkinnedMeshRenderer>().sharedMesh
			},
			new MeshReplacement
			{
				renderer = baseRendererInfos[2].renderer,
				mesh = ((Component)component9.FindChild("WingsAnimated")).GetComponent<SkinnedMeshRenderer>().sharedMesh
			},
			new MeshReplacement
			{
				renderer = baseRendererInfos[3].renderer,
				mesh = ((Component)component9.FindChild("Crown")).GetComponent<SkinnedMeshRenderer>().sharedMesh
			}
		};
		val8.Name = "AURIELBODY_DEFAULT_SKIN_NAME";
		val8.NameToken = "AURIELBODY_DEFAULT_SKIN_NAME";
		val8.RendererInfos = component8.baseRendererInfos;
		val8.RootObject = gameObject;
		val8.UnlockableDef = null;
		RendererInfo[] rendererInfos = val8.RendererInfos;
		RendererInfo[] array = (RendererInfo[])(object)new RendererInfo[rendererInfos.Length];
		rendererInfos.CopyTo(array, 0);
		Material defaultMaterial = array[0].defaultMaterial;
		bool flag = Object.op_Implicit((Object)(object)defaultMaterial);
		if (flag)
		{
			defaultMaterial = new Material(val7);
			defaultMaterial.SetColor("_Color", Color.white);
			defaultMaterial.SetTexture("_MainTex", MainAssetBundle.LoadAsset<Material>("Body").GetTexture("_MainTex"));
			defaultMaterial.SetColor("_EmColor", Color.black);
			defaultMaterial.SetFloat("_EmPower", 0f);
			defaultMaterial.SetFloat("_NormalStrength", 1f);
			defaultMaterial.SetTexture("_NormalTex", MainAssetBundle.LoadAsset<Material>("Body").GetTexture("_BumpMap"));
			array[0].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[1].defaultMaterial;
		bool flag2 = Object.op_Implicit((Object)(object)defaultMaterial);
		if (flag2)
		{
			defaultMaterial = MainAssetBundle.LoadAsset<Material>("wingmat");
			array[1].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[2].defaultMaterial;
		bool flag3 = Object.op_Implicit((Object)(object)defaultMaterial);
		if (flag3)
		{
			defaultMaterial = MainAssetBundle.LoadAsset<Material>("wingmatanim");
			array[2].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[3].defaultMaterial;
		if (Object.op_Implicit((Object)(object)defaultMaterial))
		{
			defaultMaterial = new Material(val7);
			defaultMaterial.SetColor("_Color", new Color(1f, 0.8f, 0.12f));
			defaultMaterial.SetTexture("_MainTex", (Texture)null);
			defaultMaterial.SetColor("_EmColor", Color.yellow);
			defaultMaterial.SetFloat("_EmPower", 1.4f);
			defaultMaterial.SetFloat("_NormalStrength", 1f);
			array[3].defaultMaterial = defaultMaterial;
		}
		val8.RendererInfos = array;
		SkinDef val9 = Skins.CreateNewSkinDef(val8);
		SkinDefInfo val10 = default(SkinDefInfo);
		val10.BaseSkins = Array.Empty<SkinDef>();
		val10.MinionSkinReplacements = (MinionSkinReplacement[])(object)new MinionSkinReplacement[0];
		val10.ProjectileGhostReplacements = (ProjectileGhostReplacement[])(object)new ProjectileGhostReplacement[0];
		val10.GameObjectActivations = (GameObjectActivation[])(object)new GameObjectActivation[0];
		val10.Icon = MainAssetBundle.LoadAsset<Sprite>("portraitarchangel");
		val10.MeshReplacements = (MeshReplacement[])(object)new MeshReplacement[1]
		{
			new MeshReplacement
			{
				renderer = (Renderer)(object)fieldValue,
				mesh = MainAssetBundle.LoadAsset<Mesh>("Archangel")
			}
		};
		val10.Name = "AURIELBODY_ARCHANGEL_SKIN_NAME";
		val10.NameToken = "AURIELBODY_ARCHANGEL_SKIN_NAME";
		val10.RendererInfos = component8.baseRendererInfos;
		val10.RootObject = gameObject;
		val10.UnlockableDef = Achievements.aurielMasteryDef;
		rendererInfos = val8.RendererInfos;
		array = (RendererInfo[])(object)new RendererInfo[rendererInfos.Length];
		rendererInfos.CopyTo(array, 0);
		defaultMaterial = array[0].defaultMaterial;
		if (flag)
		{
			defaultMaterial = new Material(val7);
			defaultMaterial.SetColor("_Color", Color.white);
			defaultMaterial.SetTexture("_MainTex", MainAssetBundle.LoadAsset<Material>("archangel").GetTexture("_MainTex"));
			defaultMaterial.SetColor("_EmColor", Color.yellow);
			defaultMaterial.SetFloat("_EmPower", 1f);
			defaultMaterial.SetTexture("_EmTex", MainAssetBundle.LoadAsset<Material>("archangel").GetTexture("_EmissionMap"));
			defaultMaterial.SetFloat("_NormalStrength", 1f);
			defaultMaterial.SetTexture("_NormalTex", MainAssetBundle.LoadAsset<Material>("archangel").GetTexture("_BumpMap"));
			array[0].defaultMaterial = defaultMaterial;
		}
		val10.RendererInfos = array;
		SkinDef val11 = Skins.CreateNewSkinDef(val10);
		SkinDefInfo val12 = default(SkinDefInfo);
		val12.BaseSkins = Array.Empty<SkinDef>();
		val12.MinionSkinReplacements = (MinionSkinReplacement[])(object)new MinionSkinReplacement[0];
		val12.ProjectileGhostReplacements = (ProjectileGhostReplacement[])(object)new ProjectileGhostReplacement[0];
		val12.GameObjectActivations = (GameObjectActivation[])(object)new GameObjectActivation[0];
		val12.Icon = MainAssetBundle.LoadAsset<Sprite>("portraithanamura");
		val12.MeshReplacements = (MeshReplacement[])(object)new MeshReplacement[1]
		{
			new MeshReplacement
			{
				renderer = (Renderer)(object)fieldValue,
				mesh = MainAssetBundle.LoadAsset<Mesh>("Sakura")
			}
		};
		val12.Name = "AURIELBODY_SAKURA_SKIN_NAME";
		val12.NameToken = "AURIELBODY_SAKURA_SKIN_NAME";
		val12.RendererInfos = component8.baseRendererInfos;
		val12.RootObject = gameObject;
		val12.UnlockableDef = Achievements.aurielSakuraDef;
		rendererInfos = val8.RendererInfos;
		array = (RendererInfo[])(object)new RendererInfo[rendererInfos.Length];
		rendererInfos.CopyTo(array, 0);
		defaultMaterial = array[0].defaultMaterial;
		if (flag)
		{
			defaultMaterial = new Material(val7);
			defaultMaterial.SetColor("_Color", Color.white);
			defaultMaterial.SetTexture("_MainTex", MainAssetBundle.LoadAsset<Material>("sakura").GetTexture("_MainTex"));
			defaultMaterial.SetColor("_EmColor", Color.cyan);
			defaultMaterial.SetFloat("_EmPower", 1f);
			defaultMaterial.SetTexture("_EmTex", MainAssetBundle.LoadAsset<Material>("sakura").GetTexture("_EmissionMap"));
			defaultMaterial.SetFloat("_NormalStrength", 1f);
			defaultMaterial.SetTexture("_NormalTex", MainAssetBundle.LoadAsset<Material>("sakura").GetTexture("_BumpMap"));
			array[0].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[2].defaultMaterial;
		if (flag3)
		{
			defaultMaterial = MainAssetBundle.LoadAsset<Material>("sakurawingsanim");
			array[2].defaultMaterial = defaultMaterial;
		}
		val12.RendererInfos = array;
		SkinDef val13 = Skins.CreateNewSkinDef(val12);
		SkinDefInfo val14 = default(SkinDefInfo);
		val14.BaseSkins = Array.Empty<SkinDef>();
		val14.MinionSkinReplacements = (MinionSkinReplacement[])(object)new MinionSkinReplacement[0];
		val14.ProjectileGhostReplacements = (ProjectileGhostReplacement[])(object)new ProjectileGhostReplacement[0];
		val14.GameObjectActivations = (GameObjectActivation[])(object)new GameObjectActivation[0];
		val14.Icon = MainAssetBundle.LoadAsset<Sprite>("portraitspirit");
		val14.MeshReplacements = (MeshReplacement[])(object)new MeshReplacement[3]
		{
			new MeshReplacement
			{
				renderer = (Renderer)(object)fieldValue,
				mesh = MainAssetBundle.LoadAsset<Mesh>("Spirit")
			},
			new MeshReplacement
			{
				renderer = array[1].renderer,
				mesh = MainAssetBundle.LoadAsset<Mesh>("SpiritWings")
			},
			new MeshReplacement
			{
				renderer = array[2].renderer,
				mesh = MainAssetBundle.LoadAsset<Mesh>("SpiritWings")
			}
		};
		val14.Name = "AURIELBODY_SPIRIT_SKIN_NAME";
		val14.NameToken = "AURIELBODY_SPIRIT_SKIN_NAME";
		val14.RendererInfos = component8.baseRendererInfos;
		val14.RootObject = gameObject;
		val14.UnlockableDef = Achievements.aurielSpiritDef;
		rendererInfos = val8.RendererInfos;
		array = (RendererInfo[])(object)new RendererInfo[rendererInfos.Length];
		rendererInfos.CopyTo(array, 0);
		defaultMaterial = array[0].defaultMaterial;
		if (flag)
		{
			defaultMaterial = new Material(val7);
			defaultMaterial.SetColor("_Color", Color.white);
			defaultMaterial.SetTexture("_MainTex", MainAssetBundle.LoadAsset<Material>("spirit").GetTexture("_MainTex"));
			defaultMaterial.SetColor("_EmColor", Color.cyan);
			defaultMaterial.SetFloat("_EmPower", 1f);
			defaultMaterial.SetTexture("_EmTex", MainAssetBundle.LoadAsset<Material>("spirit").GetTexture("_EmissionMap"));
			defaultMaterial.SetFloat("_NormalStrength", 1f);
			defaultMaterial.SetTexture("_NormalTex", MainAssetBundle.LoadAsset<Material>("spirit").GetTexture("_BumpMap"));
			array[0].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[2].defaultMaterial;
		if (flag3)
		{
			defaultMaterial = MainAssetBundle.LoadAsset<Material>("spiritwingsanim");
			array[2].defaultMaterial = defaultMaterial;
		}
		val14.RendererInfos = array;
		SkinDef val15 = Skins.CreateNewSkinDef(val14);
		SkinDefInfo val16 = default(SkinDefInfo);
		val16.BaseSkins = Array.Empty<SkinDef>();
		val16.MinionSkinReplacements = (MinionSkinReplacement[])(object)new MinionSkinReplacement[0];
		val16.ProjectileGhostReplacements = (ProjectileGhostReplacement[])(object)new ProjectileGhostReplacement[0];
		val16.GameObjectActivations = (GameObjectActivation[])(object)new GameObjectActivation[0];
		val16.Icon = MainAssetBundle.LoadAsset<Sprite>("portraitdemonic");
		val16.MeshReplacements = (MeshReplacement[])(object)new MeshReplacement[3]
		{
			new MeshReplacement
			{
				renderer = (Renderer)(object)fieldValue,
				mesh = MainAssetBundle.LoadAsset<Mesh>("Demonic")
			},
			new MeshReplacement
			{
				renderer = array[1].renderer,
				mesh = MainAssetBundle.LoadAsset<Mesh>("DemonicWings")
			},
			new MeshReplacement
			{
				renderer = array[2].renderer,
				mesh = MainAssetBundle.LoadAsset<Mesh>("DemonicWings")
			}
		};
		val16.Name = "AURIELBODY_DEMONIC_SKIN_NAME";
		val16.NameToken = "AURIELBODY_DEMONIC_SKIN_NAME";
		val16.RendererInfos = component8.baseRendererInfos;
		val16.RootObject = gameObject;
		val16.UnlockableDef = Achievements.aurielDemonicDef;
		rendererInfos = val8.RendererInfos;
		array = (RendererInfo[])(object)new RendererInfo[rendererInfos.Length];
		rendererInfos.CopyTo(array, 0);
		defaultMaterial = array[0].defaultMaterial;
		if (flag)
		{
			defaultMaterial = new Material(val7);
			defaultMaterial.SetColor("_Color", Color.white);
			defaultMaterial.SetTexture("_MainTex", MainAssetBundle.LoadAsset<Material>("demonic").GetTexture("_MainTex"));
			defaultMaterial.SetColor("_EmColor", Color.red);
			defaultMaterial.SetFloat("_EmPower", 1f);
			defaultMaterial.SetTexture("_EmTex", MainAssetBundle.LoadAsset<Material>("demonic").GetTexture("_EmissionMap"));
			defaultMaterial.SetFloat("_NormalStrength", 1f);
			defaultMaterial.SetTexture("_NormalTex", MainAssetBundle.LoadAsset<Material>("demonic").GetTexture("_BumpMap"));
			array[0].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[1].defaultMaterial;
		if (flag2)
		{
			defaultMaterial = MainAssetBundle.LoadAsset<Material>("demonicwings");
			array[1].defaultMaterial = defaultMaterial;
		}
		defaultMaterial = array[2].defaultMaterial;
		if (flag3)
		{
			defaultMaterial = MainAssetBundle.LoadAsset<Material>("demonicwingsanim");
			array[2].defaultMaterial = defaultMaterial;
		}
		val16.RendererInfos = array;
		SkinDef val17 = Skins.CreateNewSkinDef(val16);
		val6.skins = (SkinDef[])(object)new SkinDef[5] { val9, val11, val13, val15, val17 };
		TeamComponent val18 = null;
		val18 = ((!((Object)(object)characterPrefab.GetComponent<TeamComponent>() != (Object)null)) ? characterPrefab.GetComponent<TeamComponent>() : characterPrefab.GetComponent<TeamComponent>());
		val18.hideAllyCardDisplay = false;
		val18.teamIndex = (TeamIndex)(-1);
		HealthComponent component10 = characterPrefab.GetComponent<HealthComponent>();
		component10.health = 110f;
		component10.shield = 0f;
		component10.barrier = 0f;
		component10.magnetiCharge = 0f;
		component10.body = null;
		component10.dontShowHealthbar = false;
		characterPrefab.GetComponent<Interactor>().maxInteractionDistance = 3f;
		characterPrefab.GetComponent<InteractionDriver>().highlightInteractor = true;
		CharacterDeathBehavior component11 = characterPrefab.GetComponent<CharacterDeathBehavior>();
		component11.deathStateMachine = characterPrefab.GetComponent<EntityStateMachine>();
		component11.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));
		SfxLocator component12 = characterPrefab.GetComponent<SfxLocator>();
		component12.deathSound = "Play_ui_player_death";
		component12.barkSound = "";
		component12.openSound = "";
		component12.landingSound = "Play_char_land";
		component12.fallDamageSound = "Play_char_land_fall_damage";
		component12.aliveLoopStart = "";
		component12.aliveLoopStop = "";
		Rigidbody component13 = characterPrefab.GetComponent<Rigidbody>();
		component13.mass = 100f;
		component13.drag = 0f;
		component13.angularDrag = 0f;
		component13.useGravity = false;
		component13.isKinematic = true;
		component13.interpolation = (RigidbodyInterpolation)0;
		component13.collisionDetectionMode = (CollisionDetectionMode)0;
		component13.constraints = (RigidbodyConstraints)0;
		CapsuleCollider component14 = characterPrefab.GetComponent<CapsuleCollider>();
		((Collider)component14).isTrigger = false;
		((Collider)component14).material = null;
		component14.center = new Vector3(0f, 0f, 0f);
		component14.radius = 0.5f;
		component14.height = 2f;
		component14.direction = 1;
		KinematicCharacterMotor component15 = characterPrefab.GetComponent<KinematicCharacterMotor>();
		component15.CharacterController = (ICharacterController)(object)component4;
		component15.Capsule = component14;
		component15.playerCharacter = true;
		HurtBoxGroup val19 = val.AddComponent<HurtBoxGroup>();
		HurtBox val20 = ((Component)val.GetComponentInChildren<CapsuleCollider>()).gameObject.AddComponent<HurtBox>();
		((Component)val20).gameObject.layer = LayerIndex.entityPrecise.intVal;
		val20.healthComponent = component10;
		val20.isBullseye = true;
		val20.damageModifier = (DamageModifier)0;
		val20.hurtBoxGroup = val19;
		val20.indexInGroup = 0;
		val19.hurtBoxes = (HurtBox[])(object)new HurtBox[1] { val20 };
		val19.mainHurtBox = val20;
		val19.bullseyeCount = 1;
		HitBoxGroup val21 = gameObject.AddComponent<HitBoxGroup>();
		GameObject val22 = new GameObject("SlashHitbox");
		val22.transform.parent = ((Component)component7.FindChild("HitBoxCenter")).transform;
		val22.transform.localPosition = new Vector3(0f, 0f, 0f);
		val22.transform.localRotation = Quaternion.identity;
		val22.transform.localScale = new Vector3(10f, 5f, 10f);
		val22.layer = LayerIndex.projectile.intVal;
		HitBox val23 = val22.AddComponent<HitBox>();
		val21.hitBoxes = (HitBox[])(object)new HitBox[1] { val23 };
		val21.groupName = "SlashHitbox";
		HitBoxGroup val24 = gameObject.AddComponent<HitBoxGroup>();
		GameObject val25 = new GameObject("SlashHitboxWeakSpot");
		val25.transform.parent = ((Component)component7.FindChild("HitBoxCenter")).transform;
		val25.transform.localPosition = new Vector3(0f, 0f, 0f);
		val25.transform.localRotation = Quaternion.identity;
		val25.transform.localScale = new Vector3(4f, 5f, 10f);
		val25.layer = LayerIndex.projectile.intVal;
		HitBox val26 = val25.AddComponent<HitBox>();
		val24.hitBoxes = (HitBox[])(object)new HitBox[1] { val26 };
		val24.groupName = "SlashHitboxWeakSpot";
		HitBoxGroup val27 = gameObject.AddComponent<HitBoxGroup>();
		GameObject val28 = new GameObject("WrathHitbox");
		val28.transform.parent = ((Component)component7.FindChild("SlashHitBox")).transform;
		val28.transform.localPosition = new Vector3(0f, 0f, 0f);
		val28.transform.localRotation = Quaternion.identity;
		val28.transform.localScale = new Vector3(13f, 13f, 13f);
		val28.layer = LayerIndex.projectile.intVal;
		HitBox val29 = val28.AddComponent<HitBox>();
		val27.hitBoxes = (HitBox[])(object)new HitBox[1] { val29 };
		val27.groupName = "WrathHitbox";
		FootstepHandler val30 = val.AddComponent<FootstepHandler>();
		val30.baseFootstepString = "Play_player_footstep";
		val30.sprintFootstepOverrideString = "";
		val30.enableFootstepDust = true;
		val30.footstepDustPrefab = Load<GameObject>("RoR2/Base/Common/VFX/GenericFootstepDust.prefab");
		ContentAddition.AddBody(characterPrefab);
	}

	private void RegisterCharacter()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		characterDisplay = PrefabAPI.InstantiateClone(((Component)characterPrefab.GetComponent<ModelLocator>().modelBaseTransform).gameObject, "AurielDisplay", true);
		characterDisplay.transform.GetChild(2).localPosition = Vector3.up * 0.1f;
		characterDisplay.AddComponent<NetworkIdentity>();
		RendererInfo[] baseRendererInfos = characterDisplay.GetComponentInChildren<CharacterModel>().baseRendererInfos;
		for (int i = 0; i < baseRendererInfos.Length; i++)
		{
			baseRendererInfos[i].defaultMaterial = new Material(baseRendererInfos[i].defaultMaterial);
			baseRendererInfos[i].defaultMaterial.DisableKeyword("_DITHER");
		}
		string text = "Her eternal light illuminates even the darkest souls. Seeking harmony in all things, she is a mediator, a counselor, and when the need arises, a fearless warrior.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
		string text2 = "..and so she left, the power of hope flows into the fabric of creation.";
		LanguageAPI.Add("AURIEL_NAME", "Auriel");
		LanguageAPI.Add("AURIEL_DESCRIPTION", text);
		LanguageAPI.Add("AURIEL_SUBTITLE", "Archangel of Hope");
		LanguageAPI.Add("AURIEL_OUTRO", text2);
		LanguageAPI.Add("AURIEL_FAIL", "..and so she vanished, in the face of adversity hope's light shines the brightest.");
		SurvivorDef val = ScriptableObject.CreateInstance<SurvivorDef>();
		val.cachedName = "AURIEL_NAME";
		val.unlockableDef = Achievements.aurielUnlockDef;
		val.descriptionToken = "AURIEL_DESCRIPTION";
		val.primaryColor = characterColor;
		val.bodyPrefab = characterPrefab;
		val.displayPrefab = characterDisplay;
		val.outroFlavorToken = "AURIEL_OUTRO";
		val.mainEndingEscapeFailureFlavorToken = "AURIEL_FAIL";
		val.desiredSortPosition = 18f;
		ContentAddition.AddSurvivorDef(val);
		SkillSetup();
	}

	private void RegisterStates()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		bool flag = default(bool);
		ContentAddition.AddEntityState<SearingLight>(ref flag);
		ContentAddition.AddEntityState<SacredSweep>(ref flag);
		ContentAddition.AddEntityState<BestowHope>(ref flag);
		ContentAddition.AddEntityState<WrathofHeaven>(ref flag);
		ContentAddition.AddEntityState<AurielCharacterMain>(ref flag);
		ContentAddition.AddEntityState<SacredSweepEmp>(ref flag);
		ContentAddition.AddEntityState<BestowHopeEmp>(ref flag);
		ContentAddition.AddEntityState<Resurrect>(ref flag);
	}

	private void SkillSetup()
	{
		GenericSkill[] componentsInChildren = characterPrefab.GetComponentsInChildren<GenericSkill>();
		foreach (GenericSkill val in componentsInChildren)
		{
			Object.DestroyImmediate((Object)(object)val);
		}
		PassiveSetup();
		PrimarySetup();
		SecondarySetup();
		UtilitySetup();
		SpecialSetup();
		EmpoweredSetup();
	}

	private void EmpoweredSetup()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		LanguageAPI.Add("AURIEL_SECONDARYEMP", "<style=cIsDamage>Divine Beam</style> [<style=cIsDamage>25</style>]");
		LanguageAPI.Add("AURIEL_SECONDARYEMP_DESCRIPTION", "Auriel charges a divine power from within, <style=cIsDamage>piercing</style> enemies with a powerful beam of light dealing <style=cIsDamage>100%</style> <style=cIsDamage>+Energy</style> damage per hit and <style=cIsDamage>burns</style> their wretched flesh.");
		secondaryEmpSkillDef = ScriptableObject.CreateInstance<SkillDef>();
		secondaryEmpSkillDef.activationState = new SerializableEntityStateType(typeof(SacredSweepEmp));
		secondaryEmpSkillDef.activationStateMachineName = "Weapon";
		secondaryEmpSkillDef.baseMaxStock = 0;
		secondaryEmpSkillDef.baseRechargeInterval = 0f;
		secondaryEmpSkillDef.beginSkillCooldownOnSkillEnd = true;
		secondaryEmpSkillDef.canceledFromSprinting = false;
		secondaryEmpSkillDef.fullRestockOnAssign = false;
		secondaryEmpSkillDef.interruptPriority = (InterruptPriority)0;
		secondaryEmpSkillDef.isCombatSkill = true;
		secondaryEmpSkillDef.mustKeyPress = true;
		secondaryEmpSkillDef.cancelSprintingOnActivation = false;
		secondaryEmpSkillDef.rechargeStock = 0;
		secondaryEmpSkillDef.requiredStock = 0;
		secondaryEmpSkillDef.stockToConsume = 0;
		secondaryEmpSkillDef.icon = MainAssetBundle.LoadAsset<Sprite>("skill2emp");
		secondaryEmpSkillDef.skillDescriptionToken = "AURIEL_SECONDARYEMP_DESCRIPTION";
		secondaryEmpSkillDef.skillName = "AURIEL_SECONDARYEMP";
		secondaryEmpSkillDef.skillNameToken = "AURIEL_SECONDARYEMP";
		LanguageAPI.Add("AURIEL_UTILITYEMP", "<style=cIsDamage>Ray of Heaven</style> [<style=cIsDamage>50</style>]");
		LanguageAPI.Add("AURIEL_UTILITYEMP_DESCRIPTION", "Auriel blesses the ground with divine power, <style=cIsHealing>healing</style> her and her teammates for <style=cIsHealing>5%</style> of her max health <style=cIsDamage>+Energy</style> per second.");
		utilityEmpSkillDef = ScriptableObject.CreateInstance<SkillDef>();
		utilityEmpSkillDef.activationState = new SerializableEntityStateType(typeof(BestowHopeEmp));
		utilityEmpSkillDef.activationStateMachineName = "Weapon";
		utilityEmpSkillDef.baseMaxStock = 0;
		utilityEmpSkillDef.baseRechargeInterval = 0f;
		utilityEmpSkillDef.beginSkillCooldownOnSkillEnd = true;
		utilityEmpSkillDef.canceledFromSprinting = false;
		utilityEmpSkillDef.fullRestockOnAssign = false;
		utilityEmpSkillDef.interruptPriority = (InterruptPriority)0;
		utilityEmpSkillDef.isCombatSkill = true;
		utilityEmpSkillDef.mustKeyPress = true;
		utilityEmpSkillDef.cancelSprintingOnActivation = false;
		utilityEmpSkillDef.rechargeStock = 0;
		utilityEmpSkillDef.requiredStock = 0;
		utilityEmpSkillDef.stockToConsume = 0;
		utilityEmpSkillDef.icon = MainAssetBundle.LoadAsset<Sprite>("skill3emp");
		utilityEmpSkillDef.skillDescriptionToken = "AURIEL_UTILITYEMP_DESCRIPTION";
		utilityEmpSkillDef.skillName = "AURIEL_UTILITYEMP";
		utilityEmpSkillDef.skillNameToken = "AURIEL_UTILITYEMP";
		LanguageAPI.Add("AURIEL_SPECIALEMP", "<style=cIsDamage>Resurrect</style> [<style=cIsDamage>100</style>]");
		LanguageAPI.Add("AURIEL_SPECIALEMP_DESCRIPTION", "Auriel channels on the spirit of a fallen ally. After channeling for <style=cIsUtility>2s</style>, they are brought back to <style=cIsHealing>life</style>. If there is no ally to resurrect, Auriel <style=cIsHealing>heals</style> herself to <style=cIsHealth>full health</style>.");
		specialEmpSkillDef = ScriptableObject.CreateInstance<SkillDef>();
		specialEmpSkillDef.activationState = new SerializableEntityStateType(typeof(Resurrect));
		specialEmpSkillDef.activationStateMachineName = "Weapon";
		specialEmpSkillDef.baseMaxStock = 0;
		specialEmpSkillDef.baseRechargeInterval = 0f;
		specialEmpSkillDef.beginSkillCooldownOnSkillEnd = true;
		specialEmpSkillDef.canceledFromSprinting = false;
		specialEmpSkillDef.fullRestockOnAssign = false;
		specialEmpSkillDef.interruptPriority = (InterruptPriority)0;
		specialEmpSkillDef.isCombatSkill = true;
		specialEmpSkillDef.mustKeyPress = true;
		specialEmpSkillDef.cancelSprintingOnActivation = false;
		specialEmpSkillDef.rechargeStock = 0;
		specialEmpSkillDef.requiredStock = 0;
		specialEmpSkillDef.stockToConsume = 0;
		specialEmpSkillDef.icon = MainAssetBundle.LoadAsset<Sprite>("skill4emp");
		specialEmpSkillDef.skillDescriptionToken = "AURIEL_SPECIALEMP_DESCRIPTION";
		specialEmpSkillDef.skillName = "AURIEL_SPECIALEMP";
		specialEmpSkillDef.skillNameToken = "AURIEL_SPECIALEMP";
	}

	private void PassiveSetup()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		SkillLocator component = characterPrefab.GetComponent<SkillLocator>();
		LanguageAPI.Add("AURIEL_PASSIVE_NAME", "Angelic Flight");
		KeyCode value = EnergyKeyToggle.Value;
		LanguageAPI.Add("AURIEL_PASSIVE_DESCRIPTION", "Auriel can <style=cIsUtility>fly</style> at will. Searing Light generates <style=cIsDamage>Energy</style>, which can be used to cast <style=cIsDamage>Energy Skills</style>. Pressing " + ((object)(KeyCode)(value)).ToString() + " or Left D-Pad, enables the <style=cIsDamage>Energy Skills</style> that match their <style=cIsDamage>Energy</style> cost.");
		component.passiveSkill.enabled = true;
		component.passiveSkill.skillNameToken = "AURIEL_PASSIVE_NAME";
		component.passiveSkill.skillDescriptionToken = "AURIEL_PASSIVE_DESCRIPTION";
		component.passiveSkill.icon = MainAssetBundle.LoadAsset<Sprite>("passive");
	}

	private void PrimarySetup()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		SkillLocator component = characterPrefab.GetComponent<SkillLocator>();
		LanguageAPI.Add("AURIEL_SEARINGLIGHT", "Searing Light");
		LanguageAPI.Add("AURIEL_SEARINGLIGHT_DESCRIPTION", "Auriel fires a fiery angelic light that <style=cIsDamage>pierces</style> all enemies for <style=cIsDamage>260%</style> damage. While under the effects of Bestow Hope, Searing Light travels farther and faster.");
		SkillDef val = ScriptableObject.CreateInstance<SkillDef>();
		val.activationState = new SerializableEntityStateType(typeof(SearingLight));
		val.activationStateMachineName = "Weapon";
		val.baseMaxStock = 0;
		val.baseRechargeInterval = 0f;
		val.beginSkillCooldownOnSkillEnd = true;
		val.canceledFromSprinting = false;
		val.fullRestockOnAssign = true;
		val.interruptPriority = (InterruptPriority)0;
		val.isCombatSkill = true;
		val.mustKeyPress = false;
		val.cancelSprintingOnActivation = false;
		val.rechargeStock = 0;
		val.requiredStock = 0;
		val.stockToConsume = 0;
		val.icon = MainAssetBundle.LoadAsset<Sprite>("skill1");
		val.skillDescriptionToken = "AURIEL_SEARINGLIGHT_DESCRIPTION";
		val.skillName = "AURIEL_SEARINGLIGHT";
		val.skillNameToken = "AURIEL_SEARINGLIGHT";
		ContentAddition.AddSkillDef(val);
		component.primary = characterPrefab.AddComponent<GenericSkill>();
		SkillFamily val2 = ScriptableObject.CreateInstance<SkillFamily>();
		val2.variants = (Variant[])(object)new Variant[1];
		Reflection.SetFieldValue<SkillFamily>((object)component.primary, "_skillFamily", val2);
		SkillFamily skillFamily = component.primary.skillFamily;
		Variant[] variants = skillFamily.variants;
		Variant val3 = new Variant
		{
			skillDef = val
		};
		((Variant)(val3)).viewableNode = new Node(val.skillNameToken, false, (Node)null);
		variants[0] = val3;
		ContentAddition.AddSkillFamily(skillFamily);
	}

	private void SecondarySetup()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		SkillLocator component = characterPrefab.GetComponent<SkillLocator>();
		LanguageAPI.Add("AURIEL_SACREDSWEEP", "Sacred Sweep");
		LanguageAPI.Add("AURIEL_ACREDSWEEP_DESCRIPTION", "Auriel sweeps the area with sacred power, <style=cIsUtility>pulling</style> all nearby enemies to it's core and <style=cIsUtility>dragging</style> them along it's path. [<style=cIsDamage>Divine Beam</style>]");
		secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
		secondarySkillDef.activationState = new SerializableEntityStateType(typeof(SacredSweep));
		secondarySkillDef.activationStateMachineName = "Slide";
		secondarySkillDef.baseMaxStock = 1;
		secondarySkillDef.baseRechargeInterval = 8f;
		secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
		secondarySkillDef.canceledFromSprinting = false;
		secondarySkillDef.fullRestockOnAssign = false;
		secondarySkillDef.interruptPriority = (InterruptPriority)0;
		secondarySkillDef.isCombatSkill = true;
		secondarySkillDef.mustKeyPress = true;
		secondarySkillDef.cancelSprintingOnActivation = false;
		secondarySkillDef.rechargeStock = 1;
		secondarySkillDef.requiredStock = 1;
		secondarySkillDef.stockToConsume = 1;
		secondarySkillDef.icon = MainAssetBundle.LoadAsset<Sprite>("skill2");
		secondarySkillDef.skillDescriptionToken = "AURIEL_ACREDSWEEP_DESCRIPTION";
		secondarySkillDef.skillName = "AURIEL_SACREDSWEEP";
		secondarySkillDef.skillNameToken = "AURIEL_SACREDSWEEP";
		secondarySkillDef.keywordTokens = new string[2] { "AURIEL_SECONDARYEMP", "AURIEL_SECONDARYEMP_DESCRIPTION" };
		ContentAddition.AddSkillDef(secondarySkillDef);
		component.secondary = characterPrefab.AddComponent<GenericSkill>();
		SkillFamily val = ScriptableObject.CreateInstance<SkillFamily>();
		val.variants = (Variant[])(object)new Variant[1];
		Reflection.SetFieldValue<SkillFamily>((object)component.secondary, "_skillFamily", val);
		SkillFamily skillFamily = component.secondary.skillFamily;
		Variant[] variants = skillFamily.variants;
		Variant val2 = new Variant
		{
			skillDef = secondarySkillDef
		};
		((Variant)(val2)).viewableNode = new Node(secondarySkillDef.skillNameToken, false, (Node)null);
		variants[0] = val2;
		ContentAddition.AddSkillFamily(val);
	}

	private void UtilitySetup()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		SkillLocator component = characterPrefab.GetComponent<SkillLocator>();
		LanguageAPI.Add("AURIEL_BESTOWHOPE", "Bestow Hope");
		LanguageAPI.Add("AURIEL_BESTOWHOPE_DESCRIPTION", "Auriel <style=cIsHealing>recovers 5%</style> of her <style=cIsHealth>health</style> and gains <style=cIsDamage>25% attack speed</style> and <style=cIsUtility>25% movement speed</style> for a <style=cIsUtility>4s</style>. [<style=cIsDamage>Ray of Heaven</style>]");
		utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
		utilitySkillDef.activationState = new SerializableEntityStateType(typeof(BestowHope));
		utilitySkillDef.activationStateMachineName = "Slide";
		utilitySkillDef.baseMaxStock = 1;
		utilitySkillDef.baseRechargeInterval = 10f;
		utilitySkillDef.beginSkillCooldownOnSkillEnd = true;
		utilitySkillDef.canceledFromSprinting = false;
		utilitySkillDef.fullRestockOnAssign = false;
		utilitySkillDef.interruptPriority = (InterruptPriority)0;
		utilitySkillDef.isCombatSkill = false;
		utilitySkillDef.mustKeyPress = true;
		utilitySkillDef.cancelSprintingOnActivation = false;
		utilitySkillDef.rechargeStock = 1;
		utilitySkillDef.requiredStock = 1;
		utilitySkillDef.stockToConsume = 1;
		utilitySkillDef.icon = MainAssetBundle.LoadAsset<Sprite>("skill3");
		utilitySkillDef.skillDescriptionToken = "AURIEL_BESTOWHOPE_DESCRIPTION";
		utilitySkillDef.skillName = "AURIEL_BESTOWHOPE";
		utilitySkillDef.skillNameToken = "AURIEL_BESTOWHOPE";
		utilitySkillDef.keywordTokens = new string[2] { "AURIEL_UTILITYEMP", "AURIEL_UTILITYEMP_DESCRIPTION" };
		ContentAddition.AddSkillDef(utilitySkillDef);
		component.utility = characterPrefab.AddComponent<GenericSkill>();
		SkillFamily val = ScriptableObject.CreateInstance<SkillFamily>();
		val.variants = (Variant[])(object)new Variant[1];
		Reflection.SetFieldValue<SkillFamily>((object)component.utility, "_skillFamily", val);
		SkillFamily skillFamily = component.utility.skillFamily;
		Variant[] variants = skillFamily.variants;
		Variant val2 = new Variant
		{
			skillDef = utilitySkillDef
		};
		((Variant)(val2)).viewableNode = new Node(utilitySkillDef.skillNameToken, false, (Node)null);
		variants[0] = val2;
		ContentAddition.AddSkillFamily(val);
	}

	private void SpecialSetup()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		SkillLocator component = characterPrefab.GetComponent<SkillLocator>();
		LanguageAPI.Add("AURIEL_WRATHOFHEAVEN", "Wrath of Heaven");
		LanguageAPI.Add("AURIEL_WRATHOFHEAVEN_DESCRIPTION", "Auriel channels a ray from heaven, granting her <style=cIsDamage>divine protection</style> and deals <style=cIsDamage>400%</style> damage per hit to nearby enemies. At the end of this cast, Auriel releases a burst of fiery light around her dealing <style=cIsDamage>600%</style> damage. [<style=cIsDamage>Resurrect</style>]");
		specialSkillDef = ScriptableObject.CreateInstance<SkillDef>();
		specialSkillDef.activationState = new SerializableEntityStateType(typeof(WrathofHeaven));
		specialSkillDef.activationStateMachineName = "Weapon";
		specialSkillDef.baseMaxStock = 1;
		specialSkillDef.baseRechargeInterval = 12f;
		specialSkillDef.beginSkillCooldownOnSkillEnd = true;
		specialSkillDef.canceledFromSprinting = false;
		specialSkillDef.fullRestockOnAssign = false;
		specialSkillDef.interruptPriority = (InterruptPriority)1;
		specialSkillDef.isCombatSkill = true;
		specialSkillDef.mustKeyPress = true;
		specialSkillDef.cancelSprintingOnActivation = false;
		specialSkillDef.rechargeStock = 1;
		specialSkillDef.requiredStock = 1;
		specialSkillDef.stockToConsume = 1;
		specialSkillDef.icon = MainAssetBundle.LoadAsset<Sprite>("skill4");
		specialSkillDef.skillDescriptionToken = "AURIEL_WRATHOFHEAVEN_DESCRIPTION";
		specialSkillDef.skillName = "AURIEL_WRATHOFHEAVEN";
		specialSkillDef.skillNameToken = "AURIEL_WRATHOFHEAVEN";
		specialSkillDef.keywordTokens = new string[2] { "AURIEL_SPECIALEMP", "AURIEL_SPECIALEMP_DESCRIPTION" };
		ContentAddition.AddSkillDef(specialSkillDef);
		component.special = characterPrefab.AddComponent<GenericSkill>();
		SkillFamily val = ScriptableObject.CreateInstance<SkillFamily>();
		val.variants = (Variant[])(object)new Variant[1];
		Reflection.SetFieldValue<SkillFamily>((object)component.special, "_skillFamily", val);
		SkillFamily skillFamily = component.special.skillFamily;
		Variant[] variants = skillFamily.variants;
		Variant val2 = new Variant
		{
			skillDef = specialSkillDef
		};
		((Variant)(val2)).viewableNode = new Node(specialSkillDef.skillNameToken, false, (Node)null);
		variants[0] = val2;
		ContentAddition.AddSkillFamily(val);
	}

	private void CreateDoppelganger()
	{
		doppelganger = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/Commando/CommandoMonsterMaster.prefab"), "AurielMaster", true);
		ContentAddition.AddMaster(doppelganger);
		CharacterMaster component = doppelganger.GetComponent<CharacterMaster>();
		component.bodyPrefab = characterPrefab;
	}
}
internal class AurielCharacterMain : GenericCharacterMain
{
	private bool _providingAntiGravity;

	public float duration;

	public float acceleration;

	public float boostSpeedMultiplier = 3f;

	public float boostCooldown = 0.5f;

	private float stopwatch;

	private bool _providingFlight;

	private ICharacterGravityParameterProvider targetCharacterGravityParameterProvider;

	private ICharacterFlightParameterProvider targetCharacterFlightParameterProvider;

	private float boostCooldownTimer;

	public GenericSkill secondarySkillSlot;

	public GenericSkill utilitySkillSlot;

	public GenericSkill specialSkillSlot;

	public GenericSkill secondarySkillSlot2;

	public GenericSkill utilitySkillSlot2;

	public GenericSkill specialSkillSlot2;

	public static bool playSound = true;

	private static float EnergyValue;

	private EnergyController energyController;

	private GameObject crown;

	private bool providingAntiGravity
	{
		get
		{
			return _providingAntiGravity;
		}
		set
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (_providingAntiGravity != value)
			{
				_providingAntiGravity = value;
				if (targetCharacterGravityParameterProvider != null)
				{
					CharacterGravityParameters gravityParameters = targetCharacterGravityParameterProvider.gravityParameters;
					gravityParameters.channeledAntiGravityGranterCount += (_providingAntiGravity ? 1 : (-1));
					targetCharacterGravityParameterProvider.gravityParameters = gravityParameters;
				}
			}
		}
	}

	private bool providingFlight
	{
		get
		{
			return _providingFlight;
		}
		set
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (_providingFlight != value)
			{
				_providingFlight = value;
				if (targetCharacterFlightParameterProvider != null)
				{
					CharacterFlightParameters flightParameters = targetCharacterFlightParameterProvider.flightParameters;
					flightParameters.channeledFlightGranterCount += (_providingFlight ? 1 : (-1));
					targetCharacterFlightParameterProvider.flightParameters = flightParameters;
				}
			}
		}
	}

	private void StartFlight()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		providingAntiGravity = true;
		providingFlight = true;
		if (((EntityState)this).characterBody.hasEffectiveAuthority && Object.op_Implicit((Object)(object)((EntityState)this).characterBody.characterMotor) && ((EntityState)this).characterBody.characterMotor.isGrounded)
		{
			Vector3 velocity = ((EntityState)this).characterBody.characterMotor.velocity;
			velocity.y = 15f;
			((EntityState)this).characterBody.characterMotor.velocity = velocity;
			((BaseCharacterController)((EntityState)this).characterBody.characterMotor).Motor.ForceUnground(0.1f);
		}
	}

	public override void OnEnter()
	{
		((GenericCharacterMain)this).OnEnter();
		energyController = ((Component)((EntityState)this).characterBody).GetComponent<EnergyController>();
		if (Object.op_Implicit((Object)(object)((EntityState)this).characterBody))
		{
			targetCharacterGravityParameterProvider = ((Component)((EntityState)this).characterBody).GetComponent<ICharacterGravityParameterProvider>();
			targetCharacterFlightParameterProvider = ((Component)((EntityState)this).characterBody).GetComponent<ICharacterFlightParameterProvider>();
			StartFlight();
		}
		crown = ((Component)((BaseState)this).FindModelChild("Crown")).gameObject;
	}

	public override void Update()
	{
		((GenericCharacterMain)this).Update();
		if (((EntityState)this).characterBody.characterMotor.disableAirControlUntilCollision)
		{
			providingAntiGravity = false;
			providingFlight = false;
		}
		else
		{
			providingAntiGravity = true;
			providingFlight = true;
		}
	}

	public override void FixedUpdate()
	{
		((GenericCharacterMain)this).FixedUpdate();
		if (((EntityState)this).characterBody.HasBuff(Auriel.crownBuff))
		{
			crown.SetActive(true);
		}
		else
		{
			crown.SetActive(false);
		}
	}
}
internal class EnergyController : MonoBehaviour
{
	private CharacterBody body;

	private GameObject energyBar;

	[SerializeField]
	private float energyValue = 0f;

	private float maxEnergyValue = 1f;

	public bool canExecute = true;

	public bool toggle = true;

	public bool canRez;

	public HUDTracker tracker;

	public float barGain = 0.02f;

	public Player player;

	public float currentEnergyValue => energyValue;

	public void AddEnergy()
	{
		energyValue = MathF.Min(energyValue + barGain, maxEnergyValue);
		AkSoundEngine.PostEvent(Auriel.BarMax, ((Component)this).gameObject);
		if (!Object.op_Implicit((Object)(object)tracker))
		{
			Debug.LogWarning((object)"[Auriel]: No HUDTracker");
		}
		else
		{
			tracker.UpdateValues(energyValue);
		}
	}

	public void ResetValue()
	{
		energyValue = 0f;
		tracker.UpdateValues(energyValue);
	}

	private void Update()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if ((!Object.op_Implicit((Object)(object)body) || !body.hasEffectiveAuthority || !Input.GetKeyDown(Auriel.EnergyKeyToggle.Value)) && !player.GetButtonDown(103))
		{
			return;
		}
		if (toggle)
		{
			if (currentEnergyValue >= 0.25f)
			{
				AkSoundEngine.PostEvent(Auriel.Switch, ((Component)this).gameObject);
				toggle = !toggle;
				body.skillLocator.secondary.SetSkillOverride((object)((Component)this).gameObject, Auriel.secondaryEmpSkillDef, (SkillOverridePriority)4);
			}
			if (currentEnergyValue >= 0.49f)
			{
				body.skillLocator.utility.SetSkillOverride((object)((Component)this).gameObject, Auriel.utilityEmpSkillDef, (SkillOverridePriority)4);
			}
			if (currentEnergyValue >= 0.99f)
			{
				body.skillLocator.special.SetSkillOverride((object)((Component)this).gameObject, Auriel.specialEmpSkillDef, (SkillOverridePriority)4);
			}
		}
		else
		{
			toggle = !toggle;
			body.skillLocator.secondary.UnsetSkillOverride((object)((Component)this).gameObject, Auriel.secondaryEmpSkillDef, (SkillOverridePriority)4);
			body.skillLocator.utility.UnsetSkillOverride((object)((Component)this).gameObject, Auriel.utilityEmpSkillDef, (SkillOverridePriority)4);
			body.skillLocator.special.UnsetSkillOverride((object)((Component)this).gameObject, Auriel.specialEmpSkillDef, (SkillOverridePriority)4);
		}
	}

	private void Awake()
	{
		body = ((Component)this).GetComponent<CharacterBody>();
	}

	private void OnEnable()
	{
		LocalUser firstLocalUser = LocalUserManager.GetFirstLocalUser();
		NetworkUser val = ((firstLocalUser != null) ? firstLocalUser.currentNetworkUser : null);
		if (Object.op_Implicit((Object)(object)val) && val.inputPlayer != null)
		{
			player = val.inputPlayer;
		}
	}

	private void OnDisable()
	{
		if (Object.op_Implicit((Object)(object)energyBar))
		{
			Object.Destroy((Object)(object)energyBar);
		}
	}
}
internal class HUDTracker : MonoBehaviour
{
	public GameObject barRoot;

	public Image bar;

	public TextMeshProUGUI currentText;

	public TextMeshProUGUI fullText;

	public HUD hud;

	private EnergyController behaviour;

	private void Awake()
	{
		hud = ((Component)this).GetComponent<HUD>();
	}

	public void UpdateValues(float value)
	{
		if (!Object.op_Implicit((Object)(object)bar) || !Object.op_Implicit((Object)(object)currentText))
		{
			Debug.LogWarning((object)"[Auriel]: No Bar Elements");
			return;
		}
		bar.fillAmount = value;
		((TMP_Text)currentText).text = Mathf.CeilToInt(bar.fillAmount * 100f).ToString();
	}

	private void FixedUpdate()
	{
		if (!Object.op_Implicit((Object)(object)behaviour))
		{
			if (Object.op_Implicit((Object)(object)hud) && Object.op_Implicit((Object)(object)hud.targetBodyObject))
			{
				behaviour = hud.targetBodyObject.GetComponent<EnergyController>();
				if (Object.op_Implicit((Object)(object)behaviour))
				{
					behaviour.tracker = this;
					barRoot.gameObject.SetActive(true);
				}
			}
			if (barRoot.activeInHierarchy)
			{
				barRoot.gameObject.SetActive(false);
			}
		}
		else if (!barRoot.activeInHierarchy)
		{
			barRoot.gameObject.SetActive(true);
		}
	}
}
internal class BestowHope : BaseSkillState
{
	public float duration = 0.1f;

	private uint FireID;

	public override void OnEnter()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		((BaseState)this).OnEnter();
		FireID = AkSoundEngine.PostEvent(Auriel.Utility, ((Component)((EntityState)this).characterBody).gameObject);
		((EntityState)this).PlayAnimation("Gesture, Override", "Spell2", "Skill2.playbackRate", 1f, 0f);
		if (NetworkServer.active)
		{
			((EntityState)this).characterBody.AddTimedBuff(Auriel.crownBuff, 4f);
			CharacterBody component = ((EntityState)this).GetComponent<CharacterBody>();
			if (Object.op_Implicit((Object)(object)component))
			{
				HealthComponent healthComponent = component.healthComponent;
				if (Object.op_Implicit((Object)(object)healthComponent))
				{
					component.healthComponent.Heal(healthComponent.fullHealth * 0.15f + component.level * 0.015f, default(ProcChainMask), true);
				}
			}
		}
		EffectManager.SimpleMuzzleFlash(VFX.resSource, ((EntityState)this).gameObject, "Body", false);
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).fixedAge >= duration && ((EntityState)this).isAuthority)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)1;
	}
}
internal class BestowHopeEmp : BaseSkillState
{
	private float duration = 1f;

	private GameObject areaIndicatorInstance;

	private GameObject mushroomWard;

	private uint FireID;

	private float EnergyValue;

	public override void OnEnter()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		((BaseState)this).OnEnter();
		EnergyValue = ((Component)((EntityState)this).characterBody).GetComponent<EnergyController>().currentEnergyValue;
		areaIndicatorInstance = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
		areaIndicatorInstance.transform.localScale = new Vector3(10f, 10f, 10f);
		((EntityState)this).PlayAnimation("Gesture, Override", "Spell2_Emp", "Skill2.playbackRate", 2.5f, 0f);
	}

	private void UpdateAreaIndicator()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)areaIndicatorInstance))
		{
			float num = 1000f;
			RaycastHit val = default(RaycastHit);
			if (Physics.Raycast(((BaseState)this).GetAimRay(), ref val, num, LayerMask.op_Implicit(((LayerIndex)(LayerIndex.world)).mask)))
			{
				areaIndicatorInstance.transform.position = ((RaycastHit)(val)).point;
				areaIndicatorInstance.transform.up = ((RaycastHit)(val)).normal;
			}
		}
	}

	protected void Fire()
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		if (EnergyValue >= 0.99f)
		{
			FireID = AkSoundEngine.PostEvent(Auriel.HealFieldMax, ((Component)((EntityState)this).characterBody).gameObject);
		}
		else
		{
			FireID = AkSoundEngine.PostEvent(Auriel.HealField, ((Component)((EntityState)this).characterBody).gameObject);
		}
		((Component)((EntityState)this).characterBody).GetComponent<EnergyController>().ResetValue();
		EffectManager.SimpleMuzzleFlash(ArrowRain.muzzleFlashEffect, ((EntityState)this).gameObject, "MuzzleR", false);
		EffectManager.SimpleMuzzleFlash(ArrowRain.muzzleFlashEffect, ((EntityState)this).gameObject, "MuzzleL", false);
		if (!Object.op_Implicit((Object)(object)areaIndicatorInstance) || !((Object)(object)mushroomWard == (Object)null))
		{
			return;
		}
		CharacterBody component = ((EntityState)this).GetComponent<CharacterBody>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return;
		}
		HealthComponent healthComponent = component.healthComponent;
		if (Object.op_Implicit((Object)(object)healthComponent))
		{
			float num = healthComponent.fullHealth * 0.0005f;
			mushroomWard = Object.Instantiate<GameObject>(Prefabs.Load<GameObject>("RoR2/Base/MiniMushroom/MiniMushroomWard.prefab"), areaIndicatorInstance.transform.position, areaIndicatorInstance.transform.rotation);
			mushroomWard.GetComponent<TeamFilter>().teamIndex = TeamComponent.GetObjectTeam(((EntityState)this).gameObject);
			mushroomWard.AddComponent<ProjectileSimple>().lifetime = 8f;
			mushroomWard.AddComponent<ProjectileController>().ghostPrefab = Prefabs.healghost2;
			if (Object.op_Implicit((Object)(object)mushroomWard))
			{
				HealingWard component2 = mushroomWard.GetComponent<HealingWard>();
				component2.healFraction = num * (1f + EnergyValue);
				component2.healPoints = 0f;
				component2.Networkradius = 10f;
			}
			NetworkServer.Spawn(mushroomWard);
			EffectManager.SimpleEffect(VFX.healField, areaIndicatorInstance.transform.position + Vector3.up * 0.1f, areaIndicatorInstance.transform.rotation, false);
		}
	}

	public override void Update()
	{
		((EntityState)this).Update();
		UpdateAreaIndicator();
	}

	public override void OnExit()
	{
		Fire();
		((EntityState)this).GetModelAnimator().SetFloat("Skill2.playbackRate", 2f);
		mushroomWard = null;
		EnergyController component = ((EntityState)this).GetComponent<EnergyController>();
		component.toggle = !component.toggle;
		if (Object.op_Implicit((Object)(object)areaIndicatorInstance))
		{
			EntityState.Destroy((Object)(object)areaIndicatorInstance);
		}
		((EntityState)this).skillLocator.secondary.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.secondaryEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).skillLocator.utility.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.utilityEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).skillLocator.special.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.specialEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).OnExit();
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if ((((EntityState)this).isAuthority && ((EntityState)this).fixedAge >= duration) || ((EntityState)this).inputBank.skill1.down || ((EntityState)this).inputBank.skill2.down || ((EntityState)this).inputBank.skill4.down || !((EntityState)this).inputBank.skill3.down)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)3;
	}
}
internal class Achievements
{
	internal static UnlockableDef aurielUnlockDef;

	internal static UnlockableDef aurielMasteryDef;

	internal static UnlockableDef aurielSakuraDef;

	internal static UnlockableDef aurielSpiritDef;

	internal static UnlockableDef aurielDemonicDef;

	public static void RegisterUnlockables()
	{
		aurielUnlockDef = UnlockableAPI.AddUnlockable<SurvivorChallenge>((Type)null, (UnlockableDef)null);
		aurielMasteryDef = UnlockableAPI.AddUnlockable<MasteryChallenge>((Type)null, (UnlockableDef)null);
		aurielSakuraDef = UnlockableAPI.AddUnlockable<SakuraChallenge>((Type)null, (UnlockableDef)null);
		aurielSpiritDef = UnlockableAPI.AddUnlockable<SpiritChallenge>((Type)null, (UnlockableDef)null);
		aurielDemonicDef = UnlockableAPI.AddUnlockable<DemonicChallenge>((Type)null, (UnlockableDef)null);
		LanguageAPI.Add("AURIEL_UNLOCKABLE_ACHIEVEMENT_ID", "AurielSurvivorAchievement");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_REWARD_ID", "AurielSurvivorReward");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_NAME", "AurielReborn");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_ACHIEVEMENT_NAME", "A Blessing Or Just Luck?");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_ACHIEVEMENT_DESC", "Grasp the duality of divine intervention.");
		LanguageAPI.Add("AURIEL_MASTERY_ACHIEVEMENT_ID", "AurielMasteryAchievement");
		LanguageAPI.Add("AURIEL_MASTERY_REWARD_ID", "AurielMasteryReward");
		LanguageAPI.Add("AURIEL_MASTERY_NAME", "AurielMastery");
		LanguageAPI.Add("AURIEL_MASTERY_ACHIEVEMENT_NAME", "Auriel: Mastery");
		LanguageAPI.Add("AURIEL_MASTERY_ACHIEVEMENT_DESC", "As Auriel, beat the game or obliterate on Monsoon.");
		LanguageAPI.Add("AURIEL_SAKURA_ACHIEVEMENT_ID", "AurielSakuraAchievement");
		LanguageAPI.Add("AURIEL_SAKURA_REWARD_ID", "AurielSakuraReward");
		LanguageAPI.Add("AURIEL_SAKURA_NAME", "AurielSakura");
		LanguageAPI.Add("AURIEL_SAKURA_ACHIEVEMENT_NAME", "Mercury Paradise");
		LanguageAPI.Add("AURIEL_SAKURA_ACHIEVEMENT_DESC", "As Auriel, visit the garden.");
		LanguageAPI.Add("AURIEL_SPIRIT_ACHIEVEMENT_ID", "AurielSpiritAchievement");
		LanguageAPI.Add("AURIEL_SPIRIT_REWARD_ID", "AurielSpiritReward");
		LanguageAPI.Add("AURIEL_SPIRIT_NAME", "AurielSpirit");
		LanguageAPI.Add("AURIEL_SPIRIT_ACHIEVEMENT_NAME", "Soul Evaluation");
		LanguageAPI.Add("AURIEL_SPIRIT_ACHIEVEMENT_DESC", "As Auriel, be resurrected by the power of Dio.");
		LanguageAPI.Add("AURIEL_DEMONIC_ACHIEVEMENT_ID", "AurielDemonicAchievement");
		LanguageAPI.Add("AURIEL_DEMONIC_REWARD_ID", "AurielDemonicReward");
		LanguageAPI.Add("AURIEL_DEMONIC_NAME", "AurielDemonic");
		LanguageAPI.Add("AURIEL_DEMONIC_ACHIEVEMENT_NAME", "Wicked");
		LanguageAPI.Add("AURIEL_DEMONIC_ACHIEVEMENT_DESC", "As Auriel, commit an act of heresy.");
	}
}
internal class SurvivorChallenge : ModdedUnlockable
{
	public override string AchievementIdentifier { get; } = "AURIEL_UNLOCKABLE_ACHIEVEMENT_ID";


	public override string UnlockableIdentifier { get; } = "AURIEL_UNLOCKABLE_REWARD_ID";


	public override string AchievementNameToken { get; } = "AURIEL_UNLOCKABLE_ACHIEVEMENT_NAME";


	public override string UnlockableNameToken { get; } = "AURIEL_UNLOCKABLE_NAME";


	public override string AchievementDescToken { get; } = "AURIEL_UNLOCKABLE_ACHIEVEMENT_DESC";


	public override Sprite Sprite { get; } = Auriel.MainAssetBundle.LoadAsset<Sprite>("portrait");


	public override string PrerequisiteUnlockableIdentifier { get; } = "AURIEL_UNLOCKABLE_REWARD_ID";


	public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_UNLOCKABLE_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_UNLOCKABLE_ACHIEVEMENT_DESC")
	});


	public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_UNLOCKABLE_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_UNLOCKABLE_ACHIEVEMENT_DESC")
	});


	protected override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return BodyCatalog.FindBodyIndex("AurielBody");
	}

	private void OnInventoryChanged(orig_OnInventoryChanged orig, CharacterMaster self)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		orig.Invoke(self);
		if (Object.op_Implicit((Object)(object)self) && (int)self.teamIndex == 1 && Object.op_Implicit((Object)(object)self.inventory) && Object.op_Implicit((Object)(object)self.GetBody()))
		{
			ItemDef critGlasses = Items.CritGlasses;
			ItemDef clover = Items.Clover;
			if (self.inventory.GetItemCount(critGlasses) >= 1 && self.inventory.GetItemCount(clover) >= 1)
			{
				((BaseAchievement)this).Grant();
			}
		}
	}

	public override void OnInstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnInstall();
		CharacterMaster.OnInventoryChanged += new hook_OnInventoryChanged(OnInventoryChanged);
	}

	public override void OnUninstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnUninstall();
		CharacterMaster.OnInventoryChanged -= new hook_OnInventoryChanged(OnInventoryChanged);
	}
}
internal class MasteryChallenge : ModdedUnlockable
{
	public override string AchievementIdentifier { get; } = "AURIEL_MASTERY_ACHIEVEMENT_ID";


	public override string UnlockableIdentifier { get; } = "AURIEL_MASTERY_REWARD_ID";


	public override string AchievementNameToken { get; } = "AURIEL_MASTERY_ACHIEVEMENT_NAME";


	public override string UnlockableNameToken { get; } = "AURIEL_MASTERY_NAME";


	public override string AchievementDescToken { get; } = "AURIEL_MASTERY_ACHIEVEMENT_DESC";


	public override Sprite Sprite { get; } = Auriel.MainAssetBundle.LoadAsset<Sprite>("portraitarchangel");


	public override string PrerequisiteUnlockableIdentifier { get; } = "AURIEL_UNLOCKABLE_REWARD_ID";


	public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_MASTERY_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_MASTERY_ACHIEVEMENT_DESC")
	});


	public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_MASTERY_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_MASTERY_ACHIEVEMENT_DESC")
	});


	protected override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return BodyCatalog.FindBodyIndex("AurielBody");
	}

	public void ClearMonsoon(Run run, RunReport runReport)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		if (run != null && runReport != null && Object.op_Implicit((Object)(object)runReport.gameEnding) && runReport.gameEnding.isWin)
		{
			DifficultyIndex val = runReport.ruleBook.FindDifficulty();
			if ((int)val == 2 && ((BaseAchievement)this).meetsBodyRequirement)
			{
				((BaseAchievement)this).Grant();
			}
		}
	}

	public override void OnInstall()
	{
		((ModdedUnlockable)this).OnInstall();
		Run.onClientGameOverGlobal += ClearMonsoon;
	}

	public override void OnUninstall()
	{
		((ModdedUnlockable)this).OnUninstall();
		Run.onClientGameOverGlobal -= ClearMonsoon;
	}
}
internal class SakuraChallenge : ModdedUnlockable
{
	public override string AchievementIdentifier { get; } = "AURIEL_SAKURA_ACHIEVEMENT_ID";


	public override string UnlockableIdentifier { get; } = "AURIEL_SAKURA_REWARD_ID";


	public override string AchievementNameToken { get; } = "AURIEL_SAKURA_ACHIEVEMENT_NAME";


	public override string UnlockableNameToken { get; } = "AURIEL_SAKURA_NAME";


	public override string AchievementDescToken { get; } = "AURIEL_SAKURA_ACHIEVEMENT_DESC";


	public override Sprite Sprite { get; } = Auriel.MainAssetBundle.LoadAsset<Sprite>("portraithanamura");


	public override string PrerequisiteUnlockableIdentifier { get; } = "AURIEL_UNLOCKABLE_REWARD_ID";


	public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_SAKURA_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_SAKURA_ACHIEVEMENT_DESC")
	});


	public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_SAKURA_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_SAKURA_ACHIEVEMENT_DESC")
	});


	protected override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return BodyCatalog.FindBodyIndex("AurielBody");
	}

	private void SceneDirector_Start(orig_Start orig, SceneDirector self)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)self))
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (((Scene)(activeScene)).name == "wispgraveyard" && ((BaseAchievement)this).meetsBodyRequirement)
			{
				((BaseAchievement)this).Grant();
			}
		}
		orig.Invoke(self);
	}

	public override void OnInstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnInstall();
		SceneDirector.Start += new hook_Start(SceneDirector_Start);
	}

	public override void OnUninstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnUninstall();
		SceneDirector.Start -= new hook_Start(SceneDirector_Start);
	}
}
internal class SpiritChallenge : ModdedUnlockable
{
	public override string AchievementIdentifier { get; } = "AURIEL_SPIRIT_ACHIEVEMENT_ID";


	public override string UnlockableIdentifier { get; } = "AURIEL_SPIRIT_REWARD_ID";


	public override string AchievementNameToken { get; } = "AURIEL_SPIRIT_ACHIEVEMENT_NAME";


	public override string UnlockableNameToken { get; } = "AURIEL_SPIRIT_NAME";


	public override string AchievementDescToken { get; } = "AURIEL_SPIRIT_ACHIEVEMENT_DESC";


	public override Sprite Sprite { get; } = Auriel.MainAssetBundle.LoadAsset<Sprite>("portraitspirit");


	public override string PrerequisiteUnlockableIdentifier { get; } = "AURIEL_UNLOCKABLE_REWARD_ID";


	public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_SPIRIT_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_SPIRIT_ACHIEVEMENT_DESC")
	});


	public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_SPIRIT_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_SPIRIT_ACHIEVEMENT_DESC")
	});


	protected override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return BodyCatalog.FindBodyIndex("AurielBody");
	}

	private void CharacterMaster_RespawnExtraLife(orig_RespawnExtraLife orig, CharacterMaster self)
	{
		orig.Invoke(self);
		if (Object.op_Implicit((Object)(object)self) && Object.op_Implicit((Object)(object)((Component)self).GetComponent<PlayerCharacterMasterController>()) && ((BaseAchievement)this).meetsBodyRequirement)
		{
			((BaseAchievement)this).Grant();
		}
	}

	public override void OnInstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnInstall();
		CharacterMaster.RespawnExtraLife += new hook_RespawnExtraLife(CharacterMaster_RespawnExtraLife);
	}

	public override void OnUninstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnUninstall();
		CharacterMaster.RespawnExtraLife -= new hook_RespawnExtraLife(CharacterMaster_RespawnExtraLife);
	}
}
internal class DemonicChallenge : ModdedUnlockable
{
	public override string AchievementIdentifier { get; } = "AURIEL_DEMONIC_ACHIEVEMENT_ID";


	public override string UnlockableIdentifier { get; } = "AURIEL_DEMONIC_REWARD_ID";


	public override string AchievementNameToken { get; } = "AURIEL_DEMONIC_ACHIEVEMENT_NAME";


	public override string UnlockableNameToken { get; } = "AURIEL_DEMONIC_NAME";


	public override string AchievementDescToken { get; } = "AURIEL_DEMONIC_ACHIEVEMENT_DESC";


	public override Sprite Sprite { get; } = Auriel.MainAssetBundle.LoadAsset<Sprite>("portraitdemonic");


	public override string PrerequisiteUnlockableIdentifier { get; } = "AURIEL_UNLOCKABLE_REWARD_ID";


	public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_DEMONIC_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_DEMONIC_ACHIEVEMENT_DESC")
	});


	public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
	{
		Language.GetString("AURIEL_DEMONIC_ACHIEVEMENT_NAME"),
		Language.GetString("AURIEL_DEMONIC_ACHIEVEMENT_DESC")
	});


	protected override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return BodyCatalog.FindBodyIndex("AurielBody");
	}

	private void OnInventoryChanged(orig_OnInventoryChanged orig, CharacterMaster self)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		orig.Invoke(self);
		if (Object.op_Implicit((Object)(object)self) && (int)self.teamIndex == 1 && Object.op_Implicit((Object)(object)self.inventory) && Object.op_Implicit((Object)(object)self.GetBody()))
		{
			ItemDef lunarPrimaryReplacement = Items.LunarPrimaryReplacement;
			ItemDef lunarSecondaryReplacement = Items.LunarSecondaryReplacement;
			ItemDef lunarUtilityReplacement = Items.LunarUtilityReplacement;
			ItemDef lunarSpecialReplacement = Items.LunarSpecialReplacement;
			if ((self.inventory.GetItemCount(lunarPrimaryReplacement) >= 1 || self.inventory.GetItemCount(lunarSecondaryReplacement) >= 1 || self.inventory.GetItemCount(lunarUtilityReplacement) >= 1 || self.inventory.GetItemCount(lunarSpecialReplacement) >= 1) && ((BaseAchievement)this).meetsBodyRequirement)
			{
				((BaseAchievement)this).Grant();
			}
		}
	}

	public override void OnInstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnInstall();
		CharacterMaster.OnInventoryChanged += new hook_OnInventoryChanged(OnInventoryChanged);
	}

	public override void OnUninstall()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((ModdedUnlockable)this).OnUninstall();
		CharacterMaster.OnInventoryChanged -= new hook_OnInventoryChanged(OnInventoryChanged);
	}
}
internal class Languages
{
	internal static void AddLanguageSupport()
	{
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		LanguageAPI.Add("AURIELBODY_DEFAULT_SKIN_NAME", "По умолчанию", "RU");
		LanguageAPI.Add("AURIELBODY_ARCHANGEL_SKIN_NAME", "Архангел", "RU");
		LanguageAPI.Add("AURIELBODY_SAKURA_SKIN_NAME", "Сакура", "RU");
		LanguageAPI.Add("AURIELBODY_SPIRIT_SKIN_NAME", "Дух-целитель", "RU");
		LanguageAPI.Add("AURIELBODY_DEMONIC_SKIN_NAME", "Демонический", "RU");
		LanguageAPI.Add("AURIEL_NAME", "Ауриэль", "RU");
		LanguageAPI.Add("AURIEL_SUBTITLE", "Архангел Надежды", "RU");
		LanguageAPI.Add("AURIEL_FAIL", "..и так она исчезла, перед лицом невзгод свет надежды сияет ярче всего.", "RU");
		LanguageAPI.Add("AURIEL_SECONDARYEMP", "<style=cIsDamage>Божественный луч</style> [<style=cIsDamage>25</style>]", "RU");
		LanguageAPI.Add("AURIEL_SECONDARYEMP_DESCRIPTION", "Ауриэль заряжает божественную силу изнутри, <style=cIsDamage>пронзая</style> врагов мощным лучом света, нанося <style=cIsDamage>100%</style> <style=cIsDamage>+Энергия</style> урона за удар и <style=cIsDamage>сжигая</style> их проклятую плоть.", "RU");
		LanguageAPI.Add("AURIEL_UTILITYEMP", "<style=cIsDamage>Луч Небес</style> [<style=cIsDamage>50</style>]", "RU");
		LanguageAPI.Add("AURIEL_UTILITYEMP_DESCRIPTION", "Ауриэль благословляет землю божественной силой, <style=cIsHealing>исцеляя</style> себя и своих товарищей на <style=cIsHealing>5%</style> от ее максимального здоровья <style=cIsDamage>+Энергия</style> в секунду.", "RU");
		LanguageAPI.Add("AURIEL_SPECIALEMP", "<style=cIsDamage>Воскрешение</style> [<style=cIsDamage>100</style>]", "RU");
		LanguageAPI.Add("AURIEL_SPECIALEMP_DESCRIPTION", "Ауриэль соединяется с духом павшего союзника. После <style=cIsUtility>2с</style> канала они возвращаются к <style=cIsHealing>жизни</style>. Если нет союзника для воскрешения, Ауриэль <style=cIsHealing>исцеляет</style> себя до <style=cIsHealth>полного здоровья</style>.", "RU");
		LanguageAPI.Add("AURIEL_PASSIVE_NAME", "Ангельский полет", "RU");
		KeyCode value = Auriel.EnergyKeyToggle.Value;
		LanguageAPI.Add("AURIEL_PASSIVE_DESCRIPTION", "Ауриэль может <style=cIsUtility>летать</style> по своему желанию. Жгучий свет генерирует <style=cIsDamage>Энергию</style>, которая может быть использована для применения <style=cIsDamage>Энергетических навыков</style>. Нажатие " + ((object)(KeyCode)(value)).ToString() + " или Левой кнопки D-Pad активирует <style=cIsDamage>Энергетические навыки</style>, соответствующие их <style=cIsDamage>Энергетическим</style> затратам.", "RU");
		LanguageAPI.Add("AURIEL_SEARINGLIGHT", "Жгучий свет", "RU");
		LanguageAPI.Add("AURIEL_SEARINGLIGHT_DESCRIPTION", "Ауриэль выпускает огненный ангельский свет, который <style=cIsDamage>пронзает</style> всех врагов за <style=cIsDamage>260%</style> урона. Под эффектом Дарования Надежды, Жгучий свет летит дальше и быстрее.", "RU");
		LanguageAPI.Add("AURIEL_SACREDSWEEP", "Священный взмах", "RU");
		LanguageAPI.Add("AURIEL_ACREDSWEEP_DESCRIPTION", "Ауриэль подметает область священной силой, <style=cIsUtility>притягивая</style> всех ближайших врагов к своему центру и <style=cIsUtility>таща</style> их по своему пути. [<style=cIsDamage>Божественный луч</style>]", "RU");
		LanguageAPI.Add("AURIEL_BESTOWHOPE", "Дарование Надежды", "RU");
		LanguageAPI.Add("AURIEL_BESTOWHOPE_DESCRIPTION", "Ауриэль <style=cIsHealing>восстанавливает 5%</style> своего <style=cIsHealth>здоровья</style> и получает <style=cIsDamage>25% скорость атаки</style> и <style=cIsUtility>25% скорость движения</style> на <style=cIsUtility>4с</style>. [<style=cIsDamage>Луч Небес</style>]", "RU");
		LanguageAPI.Add("AURIEL_WRATHOFHEAVEN", "Гнев Небес", "RU");
		LanguageAPI.Add("AURIEL_WRATHOFHEAVEN_DESCRIPTION", "Ауриэль канализирует луч с небес, предоставляя ей <style=cIsDamage>божественную защиту</style> и нанося <style=cIsDamage>400%</style> урона за удар ближайшим врагам. В конце этого канала Ауриэль выпускает всплеск огненного света вокруг себя, нанося <style=cIsDamage>600%</style> урона. [<style=cIsDamage>Воскрешение</style>]", "RU");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_NAME", "Ауриэль Возрожденная", "RU");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_ACHIEVEMENT_NAME", "Благословение или просто удача?", "RU");
		LanguageAPI.Add("AURIEL_UNLOCKABLE_ACHIEVEMENT_DESC", "Постигните двойственность божественного вмешательства.", "RU");
		LanguageAPI.Add("AURIEL_MASTERY_NAME", "Ауриэль Мастерство", "RU");
		LanguageAPI.Add("AURIEL_MASTERY_ACHIEVEMENT_NAME", "Ауриэль, Мастерство", "RU");
		LanguageAPI.Add("AURIEL_MASTERY_ACHIEVEMENT_DESC", "Как Ауриэль, пройдите игру или уничтожьте на уровне Монсун.", "RU");
		LanguageAPI.Add("AURIEL_SAKURA_NAME", "Ауриэль Сакура", "RU");
		LanguageAPI.Add("AURIEL_SAKURA_ACHIEVEMENT_NAME", "Рай Меркурия", "RU");
		LanguageAPI.Add("AURIEL_SAKURA_ACHIEVEMENT_DESC", "Как Ауриэль, посетите сад.", "RU");
		LanguageAPI.Add("AURIEL_SPIRIT_NAME", "Ауриэль Дух", "RU");
		LanguageAPI.Add("AURIEL_SPIRIT_ACHIEVEMENT_NAME", "Оценка души", "RU");
		LanguageAPI.Add("AURIEL_SPIRIT_ACHIEVEMENT_DESC", "Как Ауриэль, будьте воскрешены силой Дио.", "RU");
		LanguageAPI.Add("AURIEL_DEMONIC_NAME", "Ауриэль Демонический", "RU");
		LanguageAPI.Add("AURIEL_DEMONIC_ACHIEVEMENT_NAME", "Злой", "RU");
		LanguageAPI.Add("AURIEL_DEMONIC_ACHIEVEMENT_DESC", "Как Ауриэль, совершите акт ереси.", "RU");
	}
}
internal class Prefabs
{
	public static GameObject projectile;

	public static GameObject projectileGhost;

	public static GameObject projectile2;

	public static GameObject projectileGrav;

	public static GameObject projectileGravInst;

	public static GameObject projectileGravInstGhost;

	public static GameObject healghost;

	public static GameObject healghost2;

	public static GameObject laserEffect;

	public static GameObject Crosshair;

	public static T Load<T>(string path)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Addressables.LoadAssetAsync<T>((object)path).WaitForCompletion();
	}

	public static void CreatePrefabs()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Expected O, but got Unknown
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Load<GameObject>("RoR2/Base/UI/HUDSimple.prefab");
		Object.DontDestroyOnLoad((Object)(object)val);
		HUDTracker hUDTracker = val.AddComponent<HUDTracker>();
		Transform val2 = val.GetComponent<ChildLocator>().FindChild("BottomLeftCluster");
		GameObject gameObject = ((Component)((Component)val2).GetComponentInChildren<HealthBar>()).gameObject;
		GameObject val3 = Object.Instantiate<GameObject>(gameObject);
		((Object)val3).name = "EnergyBar";
		val3.transform.SetParent(gameObject.transform.parent);
		val3.transform.localPosition = Vector2.op_Implicit(Vector2.one * 6f);
		val3.transform.localRotation = Quaternion.identity;
		val3.transform.localScale = Vector3.one;
		HealthBar component = val3.GetComponent<HealthBar>();
		RectTransform barContainer = component.barContainer;
		GameObject val4 = new GameObject("Bar", new Type[2]
		{
			typeof(RectTransform),
			typeof(Image)
		});
		RectTransform val5 = (RectTransform)val4.transform;
		((Transform)val5).SetParent((Transform)(object)barContainer);
		((Transform)val5).localPosition = Vector2.op_Implicit(Vector2.zero);
		((Transform)val5).localRotation = Quaternion.identity;
		((Transform)val5).localScale = Vector3.one;
		val5.sizeDelta = new Vector2(4f, 3f);
		val5.anchorMin = Vector2.zero;
		val5.anchorMax = Vector2.one;
		Image component2 = val4.GetComponent<Image>();
		((Graphic)component2).color = new Color(49f / 51f, 0.8235294f, 0f);
		component2.sprite = Auriel.MainAssetBundle.LoadAsset<Sprite>("energybar");
		component2.type = (Type)3;
		component2.fillMethod = (FillMethod)0;
		component2.fillAmount = 0f;
		hUDTracker.barRoot = val3;
		hUDTracker.bar = component2;
		hUDTracker.currentText = component.currentHealthText;
		((TMP_Text)hUDTracker.currentText).text = "0";
		((Component)((TMP_Text)hUDTracker.currentText).transform.parent).gameObject.SetActive(true);
		hUDTracker.fullText = component.fullHealthText;
		((TMP_Text)hUDTracker.fullText).text = "100";
		Object.Destroy((Object)(object)component);
		val3.SetActive(false);
		BuffDef val6 = ScriptableObject.CreateInstance<BuffDef>();
		((Object)val6).name = "BestowHope";
		val6.iconSprite = Load<Sprite>("RoR2/Base/Grandparent/texBuffOverheat.tif");
		val6.buffColor = Auriel.characterColor;
		val6.canStack = true;
		val6.isDebuff = false;
		val6.eliteDef = null;
		ContentAddition.AddBuffDef(val6);
		Auriel.crownBuff = val6;
		laserEffect = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/LaserMajorConstruct.prefab"), "AurielLaserEffect", false);
		ParticleSystemRenderer[] componentsInChildren = laserEffect.GetComponentsInChildren<ParticleSystemRenderer>();
		foreach (ParticleSystemRenderer val7 in componentsInChildren)
		{
			string name = ((Object)val7).name;
			if (name == "Hexagon" || name == "AreaIndicator" || name == "Fire, Electric")
			{
				((Renderer)val7).enabled = false;
			}
			if (name == "Flare (1)")
			{
				((Component)val7).transform.localScale = Vector3.one * 0.65f;
			}
		}
		projectileGravInstGhost = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/Grandparent/GrandparentGravSphereGhost.prefab"), "AurielProjectileGravInstGhost", false);
		ParticleSystemRenderer[] componentsInChildren2 = projectileGravInstGhost.GetComponentsInChildren<ParticleSystemRenderer>();
		foreach (ParticleSystemRenderer val8 in componentsInChildren2)
		{
			string name2 = ((Object)val8).name;
			if (name2 == "Goo, Drip")
			{
				((Renderer)val8).enabled = false;
			}
			if (name2 == "SoftGlow, Backdrop")
			{
				((Component)val8).transform.localScale = Vector3.one * 0.3f;
			}
		}
		projectileGravInst = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/Grandparent/GrandparentGravSphere.prefab"), "AurielProjectileGravInst", true);
		Material val9 = new Material(Load<Material>("RoR2/Base/Grandparent/matGrandparentGravArea.mat"));
		val9.SetTexture("_RemapTex", (Texture)(object)Load<Texture2D>("RoR2/Base/Common/ColorRamps/texRampGrandparent.png"));
		((Renderer)projectileGravInst.GetComponentInChildren<MeshRenderer>()).sharedMaterials = (Material[])(object)new Material[2]
		{
			val9,
			Load<Material>("RoR2/DLC1/MajorAndMinorConstruct/matMajorConstructAreaIndicator.mat")
		};
		projectileGravInst.GetComponent<ProjectileController>().ghostPrefab = projectileGravInstGhost;
		ProjectileSimple component3 = projectileGravInst.GetComponent<ProjectileSimple>();
		RadialForce component4 = projectileGravInst.GetComponent<RadialForce>();
		component3.desiredForwardSpeed = 10f;
		component3.lifetime = 4f;
		component4.radius = 25f;
		component4.damping = 0.5f;
		component4.forceMagnitude = -9000f;
		component4.forceCoefficientAtEdge = 0.5f;
		projectileGhost = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/MagmaWorm/MagmaOrbGhost.prefab"), "AurielProjectileGhost", true);
		projectileGhost.AddComponent<NetworkIdentity>();
		ParticleSystem[] componentsInChildren3 = projectileGhost.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem val10 in componentsInChildren3)
		{
			MainModule main = val10.main;
			((MainModule)(main)).simulationSpeed = 3f;
		}
		projectile = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/Commando/FMJRamping.prefab"), "AurielProjectile", true);
		Object.Destroy((Object)(object)projectile.GetComponent<ProjectileDeployToOwner>());
		ProjectileController component5 = projectile.GetComponent<ProjectileController>();
		component5.procCoefficient = 1f;
		component5.ghostPrefab = projectileGhost;
		ProjectileSimple component6 = projectile.GetComponent<ProjectileSimple>();
		component6.desiredForwardSpeed = 140f;
		component6.lifetime = 0.65f;
		ProjectileOverlapAttack component7 = projectile.GetComponent<ProjectileOverlapAttack>();
		component7.impactEffect = Load<GameObject>("RoR2/Base/FireballsOnHit/FireMeatBallExplosion.prefab");
		projectile2 = PrefabAPI.InstantiateClone(projectile, "AurielProjectile2", true);
		ProjectileSimple component8 = projectile2.GetComponent<ProjectileSimple>();
		component8.desiredForwardSpeed = 300f;
		component8.lifetime = 0.8f;
		ContentAddition.AddProjectile(projectile);
		ContentAddition.AddProjectile(projectile2);
		ContentAddition.AddProjectile(projectileGravInst);
		Crosshair = PrefabAPI.InstantiateClone(Load<GameObject>("RoR2/Base/UI/StandardCrosshair.prefab"), "AurielCrosshair", false);
		Object.Destroy((Object)(object)Crosshair.GetComponent<RawImage>());
		foreach (object item in Crosshair.transform)
		{
			Object.Destroy((Object)(object)((Component)(Transform)item).gameObject);
		}
		Crosshair.GetComponent<CrosshairController>().spriteSpreadPositions = Array.Empty<SpritePosition>();
		GameObject val11 = new GameObject("CrosshairImage", new Type[1] { typeof(RectTransform) });
		Transform transform = val11.transform;
		RectTransform val12 = (RectTransform)(object)((transform is RectTransform) ? transform : null);
		((Transform)val12).localPosition = Vector2.op_Implicit(new Vector2(0f, 0f));
		val12.SetSizeWithCurrentAnchors((Axis)0, Auriel.crosshairSize.Value);
		val12.SetSizeWithCurrentAnchors((Axis)1, Auriel.crosshairSize.Value);
		val11.transform.SetParent(Crosshair.transform);
		val11.AddComponent<Image>().sprite = Auriel.MainAssetBundle.LoadAsset<Sprite>("crosshair");
	}
}
public static class Extensions
{
	public static bool IsDeadAndOutOfLives(this CharacterMaster m)
	{
		CharacterBody body = m.GetBody();
		return (!Object.op_Implicit((Object)(object)body) || !body.healthComponent.alive) && m.inventory.GetItemCount(Items.ExtraLife) <= 0 && m.inventory.GetItemCount(Items.ExtraLifeVoid) <= 0;
	}
}
internal class RezSkillDef : SkillDef
{
	private class InstanceData : BaseSkillInstanceData
	{
		public EnergyController behaviour;
	}

	public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
	{
		return (BaseSkillInstanceData)(object)new InstanceData
		{
			behaviour = ((Component)skillSlot).GetComponent<EnergyController>()
		};
	}

	internal static bool IsExecutable([NotNull] GenericSkill skillSlot)
	{
		InstanceData instanceData = (InstanceData)(object)skillSlot.skillInstanceData;
		EnergyController behaviour = instanceData.behaviour;
		return behaviour.canRez;
	}

	public override bool CanExecute([NotNull] GenericSkill skillSlot)
	{
		return IsExecutable(skillSlot) && ((SkillDef)this).CanExecute(skillSlot);
	}

	public override bool IsReady([NotNull] GenericSkill skillSlot)
	{
		return ((SkillDef)this).IsReady(skillSlot) && IsExecutable(skillSlot);
	}
}
internal class SacredSweep : BaseSkillState
{
	public float duration;

	public float baseDuration = 1f;

	public GameObject hitEffectPrefab = Prefabs.Load<GameObject>("RoR2/Base/Commando/OmniExplosionVFXFMJ.prefab");

	public override void OnEnter()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		((BaseState)this).OnEnter();
		duration = baseDuration / ((BaseState)this).attackSpeedStat;
		Ray aimRay = ((BaseState)this).GetAimRay();
		if (((EntityState)this).isAuthority)
		{
			ProjectileManager.instance.FireProjectile(Prefabs.projectileGravInst, ((Ray)(aimRay)).origin, Util.QuaternionSafeLookRotation(((Ray)(aimRay)).direction), ((EntityState)this).gameObject, ((EntityState)this).characterBody.damage * 1f, -1000f, false, (DamageColorIndex)0, (GameObject)null, -1f, (DamageTypeCombo?)DamageTypeCombo.GenericSecondary);
		}
		((EntityState)this).PlayAnimation("Gesture, Override", "Spell1", "Skill1.playbackRate", duration, 0f);
	}

	public override void OnExit()
	{
		((EntityState)this).OnExit();
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).fixedAge >= duration && ((EntityState)this).isAuthority)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)0;
	}
}
public class SacredSweepEmp : BaseSkillState
{
	private float duration = 4f;

	public GameObject hitEffectPrefab = Prefabs.Load<GameObject>("RoR2/Base/Loader/OmniImpactVFXLoaderLightning.prefab");

	private Animator animator;

	private float fireTimer;

	private float fireFrequency = 0.3f;

	private float EnergyValue;

	private GameObject laserEffectInstance;

	private Transform laserEffectInstanceEndTransform;

	private EnergyController energyController;

	public override void OnEnter()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		((BaseState)this).OnEnter();
		((BaseState)this).StartAimMode(duration, false);
		energyController = ((EntityState)this).GetComponent<EnergyController>();
		EnergyValue = energyController.currentEnergyValue;
		Transform val = ((BaseState)this).FindModelChild("MuzzleBeam");
		laserEffectInstance = Object.Instantiate<GameObject>(Prefabs.laserEffect, val.position, val.rotation);
		laserEffectInstance.transform.parent = val;
		laserEffectInstanceEndTransform = laserEffectInstance.GetComponent<ChildLocator>().FindChild("LaserEnd");
		if (EnergyValue >= 0.99f)
		{
			AkSoundEngine.PostEvent(Auriel.LaserMax, ((EntityState)this).gameObject);
		}
		else
		{
			AkSoundEngine.PostEvent(Auriel.Laser, ((EntityState)this).gameObject);
		}
		((EntityState)this).PlayAnimation("Arms", "Beam");
		animator = ((EntityState)this).GetModelAnimator();
		energyController.ResetValue();
	}

	public override void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		((EntityState)this).Update();
		if (Object.op_Implicit((Object)(object)laserEffectInstanceEndTransform))
		{
			laserEffectInstanceEndTransform.position = GetBeamEndPoint();
		}
	}

	protected Vector3 GetBeamEndPoint()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		Ray aimRay = ((BaseState)this).GetAimRay();
		Vector3 point = ((Ray)(aimRay)).GetPoint(1000f);
		RaycastHit val = default(RaycastHit);
		if (Util.CharacterRaycast(((EntityState)this).gameObject, aimRay, ref val, 1000f, LayerMask.op_Implicit(LayerMask.op_Implicit(((LayerIndex)(LayerIndex.world)).mask) | LayerMask.op_Implicit(((LayerIndex)(LayerIndex.entityPrecise)).mask)), (QueryTriggerInteraction)0))
		{
			point = ((RaycastHit)(val)).point;
		}
		return point;
	}

	private void Attack()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		if (((EntityState)this).isAuthority)
		{
			Ray aimRay = ((BaseState)this).GetAimRay();
			new BulletAttack
			{
				falloffModel = (FalloffModel)1,
				stopperMask = ((LayerIndex)(LayerIndex.world)).mask,
				owner = ((EntityState)this).gameObject,
				weapon = ((EntityState)this).gameObject,
				origin = ((Ray)(aimRay)).origin,
				aimVector = ((Ray)(aimRay)).direction,
				minSpread = 0f,
				maxSpread = 0f,
				bulletCount = 1u,
				damage = ((BaseState)this).damageStat * (1f + EnergyValue),
				force = 40f,
				tracerEffectPrefab = null,
				muzzleName = "MuzzleBeam",
				hitEffectPrefab = hitEffectPrefab,
				isCrit = Util.CheckRoll(((BaseState)this).critStat, ((EntityState)this).characterBody.master),
				radius = 1f,
				smartCollision = true,
				damageType = (DamageTypeCombo.op_Implicit((DamageType)8192) | DamageTypeCombo.GenericSecondary)
			}.Fire();
		}
	}

	public override void OnExit()
	{
		if (Object.op_Implicit((Object)(object)laserEffectInstance))
		{
			EntityState.Destroy((Object)(object)laserEffectInstance);
		}
		animator.SetTrigger("trigger");
		EnergyController component = ((EntityState)this).GetComponent<EnergyController>();
		component.toggle = !component.toggle;
		((EntityState)this).skillLocator.secondary.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.secondaryEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).skillLocator.utility.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.utilityEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).skillLocator.special.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.specialEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).OnExit();
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		//((EntityState)this).characterBody.isSprinting = false;
		fireTimer += Time.fixedDeltaTime;
		if (fireTimer > fireFrequency / ((EntityState)this).characterBody.attackSpeed)
		{
			Attack();
			fireTimer = 0f;
		}
		if (((EntityState)this).fixedAge >= duration && ((EntityState)this).isAuthority)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)1;
	}
}
internal class SearingLight : BaseSkillState
{
	private float duration;

	private bool hasFired;

	public float baseDuration = 0.5f;

	private Animator animator;

	private ChildLocator childLocator;

	private string muzzleString;

	public static float spread = 1f;

	public int remainingShots = 2;

	private Transform muzzleTransform;

	public GameObject muzzleflashEffectPrefab = Prefabs.Load<GameObject>("RoR2/Base/Mage/MuzzleflashMageFire.prefab");

	public static GameObject projectilePrefab;

	private uint FireID;

	private float EnergyValue;

	public override void OnEnter()
	{
		((BaseState)this).OnEnter();
		EnergyValue = ((Component)((EntityState)this).characterBody).GetComponent<EnergyController>().currentEnergyValue;
		if (((EntityState)this).characterBody.HasBuff(Auriel.crownBuff))
		{
			projectilePrefab = Prefabs.projectile2;
		}
		else
		{
			projectilePrefab = Prefabs.projectile;
		}
		duration = baseDuration / ((BaseState)this).attackSpeedStat;
		((EntityState)this).characterBody.SetAimTimer(2f);
		animator = ((EntityState)this).GetModelAnimator();
		if (Object.op_Implicit((Object)(object)animator))
		{
			childLocator = ((Component)animator).GetComponent<ChildLocator>();
		}
		if (remainingShots % 2 == 0)
		{
			muzzleString = "MuzzleR";
			((EntityState)this).PlayAnimation("Gesture, Override", "Attack1", "M1.playbackRate", duration, 0f);
			FireProjectile();
		}
		else
		{
			muzzleString = "MuzzleL";
			((EntityState)this).PlayAnimation("Gesture, Override", "Attack2", "M1.playbackRate", duration, 0f);
			FireProjectile();
		}
	}

	private void FireProjectile()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		FireID = AkSoundEngine.PostEvent(Auriel.M1, ((Component)((EntityState)this).characterBody).gameObject);
		//((EntityState)this).characterBody.isSprinting = false;
		if (EnergyValue < 0.99f)
		{
			AurielCharacterMain.playSound = true;
			((EntityState)this).GetComponent<EnergyController>().AddEnergy();
		}
		if (!hasFired)
		{
			((EntityState)this).characterBody.AddSpreadBloom(spread);
			hasFired = true;
			Ray aimRay = ((BaseState)this).GetAimRay();
			if (Object.op_Implicit((Object)(object)childLocator))
			{
				muzzleTransform = childLocator.FindChild(muzzleString);
			}
			if (Object.op_Implicit((Object)(object)muzzleflashEffectPrefab))
			{
				EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, ((EntityState)this).gameObject, muzzleString, false);
			}
			if (((EntityState)this).isAuthority)
			{
				ProjectileManager.instance.FireProjectile(projectilePrefab, ((Ray)(aimRay)).origin, Util.QuaternionSafeLookRotation(((Ray)(aimRay)).direction), ((EntityState)this).gameObject, ((BaseState)this).damageStat * 2.6f, 60f, Util.CheckRoll(((BaseState)this).critStat, ((EntityState)this).characterBody.master), (DamageColorIndex)0, (GameObject)null, -1f, (DamageTypeCombo?)DamageTypeCombo.GenericPrimary);
			}
		}
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		//((EntityState)this).characterBody.isSprinting = false;
		if (!(((EntityState)this).fixedAge < duration) && ((EntityState)this).isAuthority)
		{
			remainingShots--;
			if (remainingShots == 0)
			{
				((EntityState)this).outer.SetNextStateToMain();
				return;
			}
			SearingLight searingLight = new SearingLight();
			searingLight.remainingShots = remainingShots;
			((EntityState)this).outer.SetNextState((EntityState)(object)searingLight);
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)2;
	}
}
internal static class Unlockables
{
	internal struct UnlockableInfo
	{
		internal string Name;

		internal Func<string> HowToUnlockString;

		internal Func<string> UnlockedString;

		internal int SortScore;
	}

	private static readonly HashSet<string> usedRewardIds = new HashSet<string>();

	internal static List<AchievementDef> achievementDefs = new List<AchievementDef>();

	internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();

	private static readonly List<(AchievementDef achDef, UnlockableDef unlockableDef, string unlockableName)> moddedUnlocks = new List<(AchievementDef, UnlockableDef, string)>();

	private static bool addingUnlockables;

	public static bool ableToAdd { get; private set; } = false;


	internal static UnlockableDef CreateNewUnlockable(UnlockableInfo unlockableInfo)
	{
		UnlockableDef val = ScriptableObject.CreateInstance<UnlockableDef>();
		val.nameToken = unlockableInfo.Name;
		val.cachedName = unlockableInfo.Name;
		val.getHowToUnlockString = unlockableInfo.HowToUnlockString;
		val.getUnlockedString = unlockableInfo.UnlockedString;
		val.sortScore = unlockableInfo.SortScore;
		return val;
	}

	public static UnlockableDef AddUnlockable<TUnlockable>(bool serverTracked) where TUnlockable : BaseAchievement, IModdedUnlockableDataProvider, new()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		TUnlockable val = new TUnlockable();
		string unlockableIdentifier = val.UnlockableIdentifier;
		if (!usedRewardIds.Add(unlockableIdentifier))
		{
			throw new InvalidOperationException("The unlockable identifier '" + unlockableIdentifier + "' is already used by another mod or the base game.");
		}
		AchievementDef val2 = new AchievementDef
		{
			identifier = val.AchievementIdentifier,
			unlockableRewardIdentifier = val.UnlockableIdentifier,
			prerequisiteAchievementIdentifier = val.PrerequisiteUnlockableIdentifier,
			nameToken = val.AchievementNameToken,
			descriptionToken = val.AchievementDescToken,
			achievedIcon = val.Sprite,
			type = val.GetType(),
			serverTrackerType = (serverTracked ? val.GetType() : null)
		};
		UnlockableInfo unlockableInfo = default(UnlockableInfo);
		unlockableInfo.Name = val.UnlockableIdentifier;
		unlockableInfo.HowToUnlockString = val.GetHowToUnlock;
		unlockableInfo.UnlockedString = val.GetUnlocked;
		unlockableInfo.SortScore = 200;
		UnlockableDef val3 = CreateNewUnlockable(unlockableInfo);
		unlockableDefs.Add(val3);
		achievementDefs.Add(val2);
		moddedUnlocks.Add((val2, val3, val.UnlockableIdentifier));
		if (!addingUnlockables)
		{
			addingUnlockables = true;
			AchievementManager.CollectAchievementDefs += new Manipulator(CollectAchievementDefs);
			UnlockableCatalog.Init += new Manipulator(Init_Il);
		}
		return val3;
	}

	public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target, out int index) where TDelegate : Delegate
	{
		index = cursor.EmitDelegate<TDelegate>(target);
		return cursor;
	}

	public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target) where TDelegate : Delegate
	{
		int index;
		return cursor.CallDel_(target, out index);
	}

	private static void Init_Il(ILContext il)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		new ILCursor(il).GotoNext((MoveType)1, new Func<Instruction, bool>[1]
		{
			(Instruction x) => ILPatternMatchingExt.MatchCallOrCallvirt(x, typeof(UnlockableCatalog), "SetUnlockableDefs")
		}).CallDel_(ArrayHelper.AppendDel(unlockableDefs));
	}

	private static void CollectAchievementDefs(ILContext il)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		FieldInfo field = typeof(AchievementManager).GetField("achievementIdentifiers", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if ((object)field == null)
		{
			throw new NullReferenceException("Could not find field in AchievementManager");
		}
		ILCursor val = new ILCursor(il);
		val.GotoNext((MoveType)2, new Func<Instruction, bool>[2]
		{
			(Instruction x) => ILPatternMatchingExt.MatchEndfinally(x),
			(Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 1)
		});
		val.Emit(OpCodes.Ldarg_0);
		val.Emit(OpCodes.Ldsfld, field);
		val.EmitDelegate<Action<List<AchievementDef>, Dictionary<string, AchievementDef>, List<string>>>((Action<List<AchievementDef>, Dictionary<string, AchievementDef>, List<string>>)EmittedDelegate);
		val.Emit(OpCodes.Ldloc_1);
		static void EmittedDelegate(List<AchievementDef> list, Dictionary<string, AchievementDef> map, List<string> identifiers)
		{
			ableToAdd = false;
			for (int i = 0; i < moddedUnlocks.Count; i++)
			{
				var (val2, val3, text) = moddedUnlocks[i];
				if (val2 != null)
				{
					identifiers.Add(val2.identifier);
					list.Add(val2);
					map.Add(val2.identifier, val2);
				}
			}
		}
	}
}
internal static class ArrayHelper
{
	public static T[] Append<T>(ref T[] array, List<T> list)
	{
		int num = array.Length;
		int count = list.Count;
		Array.Resize(ref array, num + count);
		list.CopyTo(array, num);
		return array;
	}

	public static Func<T[], T[]> AppendDel<T>(List<T> list)
	{
		return (T[] r) => Append(ref r, list);
	}
}
internal interface IModdedUnlockableDataProvider
{
	string AchievementIdentifier { get; }

	string UnlockableIdentifier { get; }

	string AchievementNameToken { get; }

	string PrerequisiteUnlockableIdentifier { get; }

	string UnlockableNameToken { get; }

	string AchievementDescToken { get; }

	Sprite Sprite { get; }

	Func<string> GetHowToUnlock { get; }

	Func<string> GetUnlocked { get; }
}
internal abstract class ModdedUnlockable : BaseAchievement, IModdedUnlockableDataProvider
{
	public abstract string AchievementIdentifier { get; }

	public abstract string UnlockableIdentifier { get; }

	public abstract string AchievementNameToken { get; }

	public abstract string PrerequisiteUnlockableIdentifier { get; }

	public abstract string UnlockableNameToken { get; }

	public abstract string AchievementDescToken { get; }

	public abstract Sprite Sprite { get; }

	public abstract Func<string> GetHowToUnlock { get; }

	public abstract Func<string> GetUnlocked { get; }

	protected override bool wantsBodyCallbacks => ((BaseAchievement)this).wantsBodyCallbacks;

	public void Revoke()
	{
		if (((BaseAchievement)this).userProfile.HasAchievement(AchievementIdentifier))
		{
			((BaseAchievement)this).userProfile.RevokeAchievement(AchievementIdentifier);
		}
		((BaseAchievement)this).userProfile.RevokeUnlockable(UnlockableCatalog.GetUnlockableDef(UnlockableIdentifier));
	}

	public override void OnGranted()
	{
		((BaseAchievement)this).OnGranted();
	}

	public override void OnInstall()
	{
		((BaseAchievement)this).OnInstall();
	}

	public override void OnUninstall()
	{
		((BaseAchievement)this).OnUninstall();
	}

	public override float ProgressForAchievement()
	{
		return ((BaseAchievement)this).ProgressForAchievement();
	}

	protected override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return ((BaseAchievement)this).LookUpRequiredBodyIndex();
	}

	protected override void OnBodyRequirementBroken()
	{
		((BaseAchievement)this).OnBodyRequirementBroken();
	}

	protected override void OnBodyRequirementMet()
	{
		((BaseAchievement)this).OnBodyRequirementMet();
	}
}
public class VFX
{
	public static GameObject healField;

	public static GameObject swingSlash;

	public static GameObject lightBlast;

	public static GameObject resTarget;

	public static GameObject resSource;

	public static GameObject beam;

	public static Material beamMat;

	public static Material FindMaterial(string name)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		Material[] array = Resources.FindObjectsOfTypeAll<Material>();
		for (int i = 0; i < array.Length; i++)
		{
			if ((int)((Object)array[i]).hideFlags == 0 && ((Object)array[i]).name == name)
			{
				return array[i];
			}
		}
		return null;
	}

	public static void RegisterGenericEffects()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		Wave val = default(Wave);
		val.frequency = 1f;
		val.amplitude = 1f;
		val.cycleOffset = 0f;
		Wave val2 = val;
		beam = PrefabAPI.InstantiateClone(Prefabs.Load<GameObject>("RoR2/Base/Golem/TracerGolem.prefab"), "AurielBeamEffect", true);
		beam.AddComponent<NetworkIdentity>();
		beam.AddComponent<VFXAttributes>().vfxPriority = (VFXPriority)2;
		ContentAddition.AddEffect(beam);
		lightBlast = PrefabAPI.InstantiateClone(Auriel.MainAssetBundle.LoadAsset<GameObject>("LightBlasts"), "AurielLightBlasts", true);
		lightBlast.AddComponent<DestroyOnTimer>().duration = 6f;
		lightBlast.AddComponent<NetworkIdentity>();
		GameObject val3 = Object.Instantiate<GameObject>(((Component)Prefabs.Load<GameObject>("RoR2/Base/Brother/BrotherUltLineGhost.prefab").transform.Find("Size").Find("IndicatorFX").Find("Edges")).gameObject);
		Material material = ((Renderer)val3.GetComponent<MeshRenderer>()).material;
		material.SetColor("_TintColor", Color.white);
		GameObject val4 = GameObject.CreatePrimitive((PrimitiveType)0);
		Object.Destroy((Object)(object)val4.GetComponent<SphereCollider>());
		val4.transform.localPosition = Vector3.zero;
		val4.transform.localRotation = Quaternion.identity;
		val4.transform.localScale = new Vector3(110f, 110f, 110f);
		Material val5 = new Material(Prefabs.Load<Material>("RoR2/Base/Grandparent/matGrandparentSpawnEggNet.mat"));
		val5.SetTexture("_MainTex", (Texture)(object)Prefabs.Load<Texture2D>("RoR2/Base/Common/texCloudWhitenoiseSubtle.png"));
		val5.SetTexture("_RemapTex", (Texture)(object)Prefabs.Load<Texture2D>("RoR2/Base/Common/ColorRamps/texRampParentTeleport.png"));
		val5.SetTextureScale("_MainTex", Vector2.one);
		val5.SetTextureOffset("_MainTex", Vector2.zero);
		((Renderer)val4.GetComponent<MeshRenderer>()).sharedMaterials = (Material[])(object)new Material[3]
		{
			Prefabs.Load<Material>("RoR2/Base/Grandparent/matGrandparentPortal.mat"),
			Prefabs.Load<Material>("RoR2/Base/Grandparent/matGrandparentGravArea.mat"),
			val5
		};
		val4.GetComponent<MeshFilter>().mesh.triangles.Reverse().ToArray();
		val4.GetComponent<MeshFilter>().mesh.triangles.Reverse().ToArray();
		val4.transform.SetParent(lightBlast.transform);
		resTarget = PrefabAPI.InstantiateClone(Auriel.MainAssetBundle.LoadAsset<GameObject>("ResurrectionTarget"), "AurielResurrectionTarget", true);
		resTarget.AddComponent<DestroyOnTimer>().duration = 2f;
		resTarget.AddComponent<NetworkIdentity>();
		resTarget.AddComponent<VFXAttributes>().vfxPriority = (VFXPriority)2;
		EffectComponent val6 = resTarget.AddComponent<EffectComponent>();
		val6.applyScale = false;
		val6.effectIndex = (EffectIndex)(-1);
		val6.parentToReferencedTransform = true;
		val6.positionAtReferencedTransform = true;
		val6.soundName = "";
		ContentAddition.AddEffect(resTarget);
		resSource = PrefabAPI.InstantiateClone(Auriel.MainAssetBundle.LoadAsset<GameObject>("ResurrectionAuriel"), "AurielResurrectionAuriel", true);
		resSource.AddComponent<DestroyOnTimer>().duration = 2f;
		resSource.AddComponent<NetworkIdentity>();
		resSource.AddComponent<VFXAttributes>().vfxPriority = (VFXPriority)2;
		EffectComponent val7 = resSource.AddComponent<EffectComponent>();
		val7.applyScale = false;
		val7.effectIndex = (EffectIndex)(-1);
		val7.parentToReferencedTransform = true;
		val7.positionAtReferencedTransform = true;
		val7.soundName = "";
		ContentAddition.AddEffect(resSource);
		swingSlash = PrefabAPI.InstantiateClone(Auriel.MainAssetBundle.LoadAsset<GameObject>("SwingSlash"), "AurielSwingSlash", true);
		swingSlash.AddComponent<DestroyOnTimer>().duration = 2f;
		swingSlash.AddComponent<NetworkIdentity>();
		swingSlash.AddComponent<VFXAttributes>().vfxPriority = (VFXPriority)2;
		EffectComponent val8 = swingSlash.AddComponent<EffectComponent>();
		val8.applyScale = false;
		val8.effectIndex = (EffectIndex)(-1);
		val8.parentToReferencedTransform = true;
		val8.positionAtReferencedTransform = true;
		val8.soundName = "";
		ContentAddition.AddEffect(swingSlash);
		healField = PrefabAPI.InstantiateClone(Auriel.MainAssetBundle.LoadAsset<GameObject>("HealField"), "AurielHealField", true);
		healField.AddComponent<DestroyOnTimer>().duration = 8f;
		healField.AddComponent<NetworkIdentity>();
		healField.AddComponent<VFXAttributes>().vfxPriority = (VFXPriority)2;
		EffectComponent val9 = healField.AddComponent<EffectComponent>();
		val9.applyScale = false;
		val9.effectIndex = (EffectIndex)(-1);
		val9.parentToReferencedTransform = false;
		val9.positionAtReferencedTransform = false;
		val9.soundName = "";
		ContentAddition.AddEffect(healField);
	}
}
internal class WrathofHeaven : BaseSkillState
{
	public float duration = 6f;

	public GameObject hitEffectPrefab = Prefabs.Load<GameObject>("RoR2/Base/Loader/OmniImpactVFXLoaderLightning.prefab");

	public GameObject contEffectPrefab = Prefabs.Load<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab");

	public HitBoxGroup hitBoxGroup;

	public Transform modelTransform;

	public OverlapAttack attack;

	private float fireTimer;

	private float fireFrequency = 1f;

	private Animator animator;

	private uint FireID;

	private GameObject effect;

	public override void OnEnter()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		((BaseState)this).OnEnter();
		FireID = AkSoundEngine.PostEvent(Auriel.Special, ((Component)((EntityState)this).characterBody).gameObject);
		animator = ((EntityState)this).GetModelAnimator();
		animator.SetBool("Skill3End", false);
		((EntityState)this).PlayAnimation("FullBody, Override", "Spell3Start");
		fireTimer = 0f;
		CharacterBody component = ((EntityState)this).GetComponent<CharacterBody>();
		if (Object.op_Implicit((Object)(object)component))
		{
			HealthComponent healthComponent = component.healthComponent;
			if (Object.op_Implicit((Object)(object)healthComponent))
			{
				component.healthComponent.AddBarrier(healthComponent.fullHealth * 0.35f + component.level * 0.015f);
			}
		}
		effect = Object.Instantiate<GameObject>(VFX.lightBlast, ((EntityState)this).characterBody.corePosition, Quaternion.identity, ((EntityState)this).transform);
	}

	private void Attack()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		HitBoxGroup val = null;
		Transform val2 = ((EntityState)this).GetModelTransform();
		if (Object.op_Implicit((Object)(object)val2))
		{
			hitBoxGroup = Array.Find(((Component)val2).GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "WrathHitbox");
			val = Array.Find(((Component)val2).GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "WrathHitbox");
		}
		if (((EntityState)this).isAuthority)
		{
			new BlastAttack
			{
				damageType = DamageTypeCombo.GenericSpecial,
				damageColorIndex = (DamageColorIndex)0,
				attacker = ((EntityState)this).gameObject,
				inflictor = ((EntityState)this).gameObject,
				teamIndex = ((BaseState)this).GetTeam(),
				baseDamage = ((EntityState)this).characterBody.damage * 4f,
				falloffModel = (FalloffModel)1,
				procCoefficient = 0.1f + (1f - ((BaseState)this).attackSpeedStat / 2f),
				impactEffect = EffectCatalog.FindEffectIndexFromPrefab(hitEffectPrefab),
				baseForce = 50f,
				crit = ((BaseState)this).RollCrit(),
				position = ((EntityState)this).gameObject.transform.position,
				radius = 55f
			}.Fire();
		}
	}

	private void Explosion()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		if (((EntityState)this).isAuthority)
		{
			((EntityState)this).PlayAnimation("FullBody, Override", "Spell3_End");
			new BlastAttack
			{
				damageType = DamageTypeCombo.op_Implicit((DamageType)0),
				damageColorIndex = (DamageColorIndex)0,
				attacker = ((EntityState)this).gameObject,
				inflictor = ((EntityState)this).gameObject,
				teamIndex = ((BaseState)this).GetTeam(),
				baseDamage = ((EntityState)this).characterBody.damage * 6f,
				falloffModel = (FalloffModel)0,
				procCoefficient = 1f,
				impactEffect = EffectCatalog.FindEffectIndexFromPrefab(Prefabs.Load<GameObject>("RoR2/Base/FireBallDash/FireballVehicleDamageEffect.prefab")),
				baseForce = 50f,
				crit = ((BaseState)this).RollCrit(),
				position = ((EntityState)this).gameObject.transform.position,
				radius = 25f
			}.Fire();
			EffectManager.SimpleMuzzleFlash(Prefabs.Load<GameObject>("RoR2/Base/IgniteOnKill/IgniteExplosionVFX.prefab"), ((EntityState)this).gameObject, "SlashHitBox", false);
		}
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).fixedAge >= duration && ((EntityState)this).isAuthority)
		{
			Explosion();
			if (Object.op_Implicit((Object)(object)effect))
			{
				Object.Destroy((Object)(object)effect);
			}
			animator.SetBool("Skill3End", true);
			((EntityState)this).outer.SetNextStateToMain();
		}
		if (((EntityState)this).fixedAge >= 0.8f && ((EntityState)this).inputBank.skill4.down)
		{
			Explosion();
			if (Object.op_Implicit((Object)(object)effect))
			{
				Object.Destroy((Object)(object)effect);
			}
			animator.SetBool("Skill3End", true);
			((EntityState)this).outer.SetNextStateToMain();
		}
		fireTimer += Time.fixedDeltaTime;
		float num = fireFrequency * ((BaseState)this).attackSpeedStat;
		float num2 = 0.3f / num;
		if (fireTimer > num2)
		{
			Attack();
			fireTimer = 0f;
		}
	}

	public override void OnExit()
	{
		AkSoundEngine.StopPlayingID(FireID);
		animator.SetBool("Skill3End", true);
		((EntityState)this).OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)2;
	}
}
public class Resurrect : BaseSkillState
{
	private float duration = 2f;

	private EnergyController energyController;

	public override void OnEnter()
	{
		((BaseState)this).OnEnter();
		energyController = ((Component)((EntityState)this).characterBody).GetComponent<EnergyController>();
		energyController.ResetValue();
		EffectManager.SimpleMuzzleFlash(VFX.resSource, ((EntityState)this).gameObject, "Body", false);
		AkSoundEngine.PostEvent(Auriel.res_Source, ((Component)((EntityState)this).characterBody).gameObject);
		((EntityState)this).PlayAnimation("FullBody, Override", "Resurrect", "Skill3.playbackRate", duration, 0f);
		EffectManager.SimpleMuzzleFlash(VFX.resSource, ((EntityState)this).gameObject, "SlashHitBox", false);
	}

	private void Resurrection()
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (NetworkServer.active)
        {
            // Filtra jugadores que no tienen un cuerpo físico activo en el mapa
            PlayerCharacterMasterController[] array = PlayerCharacterMasterController.instances
                .Where(x => x.master != null && x.master.GetBody() == null).ToArray();

            int num = (array != null) ? array.Length : 0;
            if (num > 0)
            {
                // Elige un aliado al azar de la lista de "sin cuerpo"
                CharacterMaster master = array[RoR2Application.rng.RangeInt(0, num)].master;
                
                // Determina la posición: si no hay registro de dónde murió, lo revive sobre Auriel
                Vector3 spawnPos = (master.deathFootPosition != Vector3.zero) ? master.deathFootPosition : ((EntityState)this).transform.position;
                
                master.Respawn(spawnPos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), true);
                
                // Avisa al motor de sonido y crea el efecto visual en la posición de respawn
                AkSoundEngine.PostEvent(Auriel.res_Target, ((Component)master).gameObject);
                EffectManager.SimpleEffect(VFX.resTarget, spawnPos, Quaternion.identity, false);
            }
            else
            {
                // Si todos están vivos, Auriel se cura a tope como premio
                ((EntityState)this).healthComponent.Heal(((EntityState)this).healthComponent.fullCombinedHealth, default(ProcChainMask), true);
            }
        }
	}

	public override void FixedUpdate()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).fixedAge >= duration && ((EntityState)this).isAuthority)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
		((EntityState)this).characterMotor.velocity = Vector3.zero;
	}

	public override void OnExit()
	{
		Resurrection();
		EnergyController component = ((EntityState)this).GetComponent<EnergyController>();
		component.toggle = !component.toggle;
		((EntityState)this).skillLocator.secondary.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.secondaryEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).skillLocator.utility.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.utilityEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).skillLocator.special.UnsetSkillOverride((object)((EntityState)this).gameObject, Auriel.specialEmpSkillDef, (SkillOverridePriority)4);
		((EntityState)this).modelLocator.normalizeToFloor = false;
		((EntityState)this).OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)7;
	}
}
