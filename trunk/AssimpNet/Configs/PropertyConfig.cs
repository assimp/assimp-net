using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp.Unmanaged;
using Assimp;

namespace Assimp.Configs {
    /// <summary>
    /// Interface describing a configuration property the Assimp importer.
    /// </summary>
    public interface IPropertyConfig {

        /// <summary>
        /// Gets the property name.
        /// </summary>
        String Name {
            get;
        }

        /// <summary>
        /// Applies the property value.
        /// </summary>
        void ApplyValue();

        /// <summary>
        /// Applies the default property, if any.
        /// </summary>
        void ApplyDefaultValue();
    }

    /// <summary>
    /// Describes an integer configuration property.
    /// </summary>
    public class IntegerPropertyConfig : IPropertyConfig {
        private String _name;
        private int _value;
        private int? _defaultValue;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public int Value {
            get {
                return _value;
            }
        }

        /// <summary>
        /// Gets the default property value, if any.
        /// </summary>
        public int? DefaultValue {
            get {
                return _defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new IntengerPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public IntegerPropertyConfig(String name, int value) {
            _name = name;
            _value = value;
            _defaultValue = null;
        }

        /// <summary>
        /// constructs a new IntegerPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public IntegerPropertyConfig(String name, int value, int defaultValue) {
            _name = name;
            _value = value;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Applies the property value.
        /// </summary>
        public void ApplyValue() {
            AssimpMethods.SetImportPropertyInteger(_name, _value);
        }

        /// <summary>
        /// Applies the default property, if any.
        /// </summary>
        public void ApplyDefaultValue() { 
            if(_defaultValue.HasValue) {
                AssimpMethods.SetImportPropertyInteger(_name, _defaultValue.Value);
            }
        }
    }

    /// <summary>
    /// Describes a float configuration property.
    /// </summary>
    public class FloatPropertyConfig : IPropertyConfig {
        private String _name;
        private float _value;
        private float? _defaultValue;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public String Name {
            get { 
                return _name;
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public float Value {
            get {
                return _value;
            }
        }

        /// <summary>
        /// Gets the default property value, if any.
        /// </summary>
        public float? DefaultValue {
            get {
                return _defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new FloatPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public FloatPropertyConfig(String name, float value) {
            _name = name;
            _value = value;
            _defaultValue = null;
        }

        /// <summary>
        /// Constructs a new FloatPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public FloatPropertyConfig(String name, float value, float defaultValue) {
            _name = name;
            _value = value;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Applies the property value.
        /// </summary>
        public void ApplyValue() {
            AssimpMethods.SetImportPropertyFloat(_name, _value);
        }

        /// <summary>
        /// Applies the default property, if any.
        /// </summary>
        public void ApplyDefaultValue() {
            if(_defaultValue.HasValue) {
                AssimpMethods.SetImportPropertyFloat(_name, _defaultValue.Value);
            }
        }
    }

    /// <summary>
    /// Describes a boolean configuration property.
    /// </summary>
    public class BooleanPropertyConfig : IPropertyConfig {
        private String _name;
        private bool _value;
        private bool? _defaultValue;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public bool Value {
            get {
                return _value;
            }
        }

        /// <summary>
        /// Gets the default property value, if any.
        /// </summary>
        public bool? DefaultValue {
            get {
                return _defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new BooleanPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public BooleanPropertyConfig(String name, bool value) {
            _name = name;
            _value = value;
            _defaultValue = null;
        }

        /// <summary>
        /// Constructs a new BooleanPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public BooleanPropertyConfig(String name, bool value, bool defaultValue) {
            _name = name;
            _value = value;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Applies the property value.
        /// </summary>
        public void ApplyValue() {
            int aiBool = (_value) ? 1 : 0;
            AssimpMethods.SetImportPropertyInteger(_name, aiBool);
        }

        /// <summary>
        /// Applies the default property, if any.
        /// </summary>
        public void ApplyDefaultValue() {
            if(_defaultValue.HasValue) {
                int aiBool = (_defaultValue.Value) ? 1 : 0;
                AssimpMethods.SetImportPropertyInteger(_name, aiBool);
            }
        }
    }

    /// <summary>
    /// Describes a string configuration property.
    /// </summary>
    public class StringPropertyConfig : IPropertyConfig {
        private String _name;
        private String _value;
        private String _defaultValue;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public String Value {
            get {
                return _value;
            }
        }

        /// <summary>
        /// Gets the default property value, if any.
        /// </summary>
        public String DefaultValue {
            get {
                return _defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new StringPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public StringPropertyConfig(String name, String value) {
            _name = name;
            _value = value;
            _defaultValue = null;
        }

        /// <summary>
        /// Constructs a new StringPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public StringPropertyConfig(String name, String value, String defaultValue) {
            _name = name;
            _value = value;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Applies the property value.
        /// </summary>
        public void ApplyValue() {
            AssimpMethods.SetImportPropertyString(_name, _value);
        }

        /// <summary>
        /// Applies the default property, if any.
        /// </summary>
        public void ApplyDefaultValue() {
            AssimpMethods.SetImportPropertyString(_name, _defaultValue);
        }

        /// <summary>
        /// Convience method for constructing a whitespace delimited name list.
        /// </summary>
        /// <param name="names">Array of names</param>
        /// <returns>White-space delimited list as a string</returns>
        protected static String ProcessNames(String[] names) {
            if(names == null || names.Length == 0) {
                return String.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach(String name in names) {
                if(!String.IsNullOrEmpty(name)) {
                    builder.Append(name);
                    builder.Append(' ');
                }
            }
            return builder.ToString();
        }
    }

    #region Library settings

    /// <summary>
    /// Configuration to enable time measurements. If enabled, each
    /// part of the loading process is timed and logged.
    /// </summary>
    public class MeasureTimeConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by MeasureTimeConfig.
        /// </summary>
        public static String MeasureTimeConfigName {
            get {
                return AiConfigs.AI_CONFIG_GLOB_MEASURE_TIME;
            }
        }

        /// <summary>
        /// Constructs a new MeasureTimeConfig.
        /// </summary>
        /// <param name="measureTime">True if the loading process should be timed or not.</param>
        public MeasureTimeConfig(bool measureTime) 
            : base(MeasureTimeConfigName, measureTime, false) { }
    }

    /// <summary>
    /// Configuration to set Assimp's multithreading policy. Possible
    /// values are -1 to let Assimp decide, 0 to disable multithreading, or
    /// any number larger than zero to force a specific number of threads. This
    /// is only a hint and may be ignored by Assimp.
    /// </summary>
    public class MultithreadingConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MultithreadingConfig.
        /// </summary>
        public static String MultithreadingConfigName {
            get {
                return AiConfigs.AI_CONFIG_GLOB_MULTITHREADING;
            }
        }
        
        /// <summary>
        /// Constructs a new MultithreadingConfig.
        /// </summary>
        /// <param name="value">A value of -1 will let Assimp decide,
        /// a value of zero to disable multithreading, and a value greater than zero
        /// to force a specific number of threads.</param>
        public MultithreadingConfig(int value) 
            : base(MultithreadingConfigName, value, -1) { }
    }

    #endregion

    #region Post Processing Settings

    /// <summary>
    /// Configuration to set the maximum angle that may be between two vertex tangents/bitangents
    /// when they are smoothed during the step to calculate the tangent basis. The default
    /// value is 45 degrees.
    /// </summary>
    public class TangentSmoothingAngleConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by TangentSmoothingAngleConfig.
        /// </summary>
        public static String TangentSmoothingAngleConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE;
            }
        }

        /// <summary>
        /// Constructs a new TangentSmoothingAngleConfig.
        /// </summary>
        /// <param name="angle">Smoothing angle, in degrees.</param>
        public TangentSmoothingAngleConfig(float angle)
            : base(TangentSmoothingAngleConfigName, Math.Min(angle, 175.0f), 45.0f) { }
    }

    /// <summary>
    /// Configuration to set the maximum angle between two face normals at a vertex when
    /// they are smoothed during the step to calculate smooth normals. This is frequently
    /// called the "crease angle". The maximum and default value is 175 degrees.
    /// </summary>
    public class NormalSmoothingAngleConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by NormalSmoothingAngleConfig.
        /// </summary>
        public static String NormalSmoothingAngleConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE;
            }
        }
        /// <summary>
        /// Constructs a new NormalSmoothingAngleConfig.
        /// </summary>
        /// <param name="angle">Smoothing angle, in degrees.</param>
        public NormalSmoothingAngleConfig(float angle)
            : base(NormalSmoothingAngleConfigName, Math.Min(angle, 175.0f), 175.0f) { }
    }

    /// <summary>
    /// Configuration to set the colormap (palette) to be used to decode embedded textures in MDL (Quake or 3DG5)
    /// files. This must be a valid path to a file. The file is 768 (256 * 3) bytes alrge and contains
    /// RGB triplets for each of the 256 palette entries. If the file is not found, a
    /// default palette (from Quake 1) is used. The default value is "colormap.lmp".
    /// </summary>
    public class MDLColorMapConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by MDLColorMapConfig.
        /// </summary>
        public static String MDLColorMapConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MDL_COLORMAP;
            }
        }

        /// <summary>
        /// Constructs a new MDLColorMapConfig.
        /// </summary>
        /// <param name="fileName">Colormap filename</param>
        public MDLColorMapConfig(String fileName) 
            : base(MDLColorMapConfigName, (String.IsNullOrEmpty(fileName)) ? "colormap.lmp" : fileName, "colormap.lmp") { }
    }

    /// <summary>
    /// Configuration for the the <see cref="PostProcessSteps.RemoveRedundantMaterials"/> step
    /// to determine what materials to keep. If a material matches one of these names it will not
    /// be modified or removed by the post processing step. Default is an empty string.
    /// </summary>
    public class MaterialExcludeListConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by MaterialExcludeListConfig.
        /// </summary>
        public static String MaterialExcludeListConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_RRM_EXCLUDE_LIST;
            }
        }

        /// <summary>
        /// Constructs a new MaterialExcludeListConfig. Material names containing whitespace
        /// <c>must</c> be enclosed in single quotation marks.
        /// </summary>
        /// <param name="materialNames">List of material names that will not be modified or replaced by the remove redundant materials post process step.</param>
        public MaterialExcludeListConfig(String[] materialNames)
            : base(MaterialExcludeListConfigName, ProcessNames(materialNames), String.Empty) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.PreTransformVertices"/> step
    /// to keep the scene hierarchy. Meshes are moved to worldspace, but no optimization is performed
    /// where meshes with the same materials are not joined. This option can be useful
    /// if you have a scene hierarchy that contains important additional information
    /// which you intend to parse. The default value is false.
    /// </summary>
    public class KeepSceneHierarchyConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by KeepSceneHierarchyConfig.
        /// </summary>
        public static String KeepSceneHierarchyConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_PTV_KEEP_HIERARCHY;
            }
        }

        /// <summary>
        /// Constructs a new KeepHierarchyConfig. 
        /// </summary>
        /// <param name="keepHierarchy">True to keep the hierarchy, false otherwise.</param>
        public KeepSceneHierarchyConfig(bool keepHierarchy)
            : base(KeepSceneHierarchyConfigName, keepHierarchy, false) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.PreTransformVertices"/> step
    /// to normalize all vertex components into the -1...1 range. The default value is
    /// false.
    /// </summary>
    public class NormalizeVertexComponentsConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by NormalizeVertexComponentsConfig.
        /// </summary>
        public static String NormalizeVertexComponentsConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_PTV_NORMALIZE;
            }
        }

        /// <summary>
        /// Constructs a new NormalizeVertexComponentsConfig.
        /// </summary>
        /// <param name="normalizeVertexComponents">True if the post process step should normalize vertex components, false otherwise.</param>
        public NormalizeVertexComponentsConfig(bool normalizeVertexComponents) 
            : base(NormalizeVertexComponentsConfigName, normalizeVertexComponents, false) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.FindDegenerates"/> step to
    /// remove degenerted primitives from the import immediately. The default value is false,
    /// where degenerated triangles are converted to lines, and degenerated lines to points.
    /// </summary>
    public class RemoveDegeneratePrimitivesConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by RemoveDegeneratePrimitivesConfig.
        /// </summary>
        public static String RemoveDegeneratePrimitivesConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_FD_REMOVE;
            }
        }

        /// <summary>
        /// Constructs a new RemoveDegeneratePrimitivesConfig.
        /// </summary>
        /// <param name="removeDegenerates">True if the post process step should remove degenerate primitives, false otherwise.</param>
        public RemoveDegeneratePrimitivesConfig(bool removeDegenerates) 
            : base (RemoveDegeneratePrimitivesConfigName, removeDegenerates, false) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.OptimizeGraph"/> step
    /// to preserve nodes matching a name in the given list. Nodes that match the names in the list
    /// will not be modified or removed. Identifiers containing whitespaces
    /// <c>must</c> be enclosed in single quotation marks. The default value is an
    /// empty string.
    /// </summary>
    public class NodeExcludeListConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by NodeExcludeListConfig.
        /// </summary>
        public static String NodeExcludeListConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_OG_EXCLUDE_LIST;
            }
        }

        /// <summary>
        /// Constructs a new NodeExcludeListConfig.
        /// </summary>
        /// <param name="nodeNames">List of node names</param>
        public NodeExcludeListConfig(params String[] nodeNames) 
            : base(NodeExcludeListConfigName, ProcessNames(nodeNames), String.Empty) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.SplitLargeMeshes"/> step 
    /// that specifies the maximum number of triangles a mesh can contain. The
    /// default value is MeshTriangleLimitConfigDefaultValue.
    /// </summary>
    public class MeshTriangleLimitConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MeshTriangleLimitConfig.
        /// </summary>
        public static String MeshTriangleLimitConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SLM_TRIANGLE_LIMIT;
            }
        }

        /// <summary>
        /// Gets the defined default limit value, this corresponds to the
        /// <see cref="AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES"/> constant.
        /// </summary>
        public static int MeshTriangleLimitConfigDefaultValue {
            get {
                return AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES;
            }
        }

        /// <summary>
        /// Constructs a new MeshTriangleLimitConfig.
        /// </summary>
        /// <param name="maxTriangleLimit">Max number of triangles a mesh can contain.</param>
        public MeshTriangleLimitConfig(int maxTriangleLimit) 
            : base(MeshTriangleLimitConfigName, maxTriangleLimit, MeshTriangleLimitConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.SplitLargeMeshes"/> step
    /// that specifies the maximum number of vertices a mesh can contain. The
    /// default value is MeshVertexLimitConfigDefaultValue.
    /// </summary>
    public class MeshVertexLimitConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MeshVertexLimitConfig.
        /// </summary>
        public static String MeshVertexLimitConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SLM_VERTEX_LIMIT;
            }
        }

        /// <summary>
        /// Gets the defined default limit value, this corresponds to the
        /// <see cref="AiDefines.AI_SLM_DEFAULT_MAX_VERTICES"/> constant.
        /// </summary>
        public static int MeshVertexLimitConfigDefaultValue {
            get {
                return AiDefines.AI_SLM_DEFAULT_MAX_VERTICES;
            }
        }

        /// <summary>
        /// Constructs a new MeshVertexLimitConfig.
        /// </summary>
        /// <param name="maxVertexLimit">Max number of vertices a mesh can contain.</param>
        public MeshVertexLimitConfig(int maxVertexLimit) 
            : base(MeshVertexLimitConfigName, maxVertexLimit, MeshVertexLimitConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.LimitBoneWeights"/> step
    /// that specifies the maximum number of bone weights per vertex. The default
    /// value is VertexBoneWeightLimitConfigDefaultValue.
    /// </summary>
    public class VertexBoneWeightLimitConfig : IntegerPropertyConfig {

        /// <summary>
        /// gets the string name used by VertexBoneWeightLimitConfig.
        /// </summary>
        public static String VertexBoneWeightLimitConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_LBW_MAX_WEIGHTS;
            }
        }

        /// <summary>
        /// Gets the defined default limit value, this corresponds to the
        /// <see cref="AiDefines.AI_LBW_MAX_WEIGHTS"/> constant.
        /// </summary>
        public static int VertexBoneWeightLimitConfigDefaultValue {
            get {
                return AiDefines.AI_LBW_MAX_WEIGHTS;
            }
        }

        /// <summary>
        /// Constructs a new VertexBoneWeightLimitConfig.
        /// </summary>
        /// <param name="maxBoneWeights">Max number of bone weights per vertex.</param>
        public VertexBoneWeightLimitConfig(int maxBoneWeights) 
            : base(VertexBoneWeightLimitConfigName, maxBoneWeights, VertexBoneWeightLimitConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.ImproveCacheLocality"/> step
    /// that specifies the size of the post-transform vertex cache. The size is
    /// given in number of vertices and the default value is VertexCacheSizeConfigDefaultValue.
    /// </summary>
    public class VertexCacheSizeConfig : IntegerPropertyConfig {
        
        /// <summary>
        /// Gets the string name used by VertexCacheConfig.
        /// </summary>
        public static String VertexCacheSizeConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_ICL_PTCACHE_SIZE;
            }
        }

        /// <summary>
        /// Gets the defined default vertex cache size, this corresponds to 
        /// the <see cref="AiDefines.PP_ICL_PTCACHE_SIZE"/>.
        /// </summary>
        public static int VertexCacheSizeConfigDefaultValue {
            get {
                return AiDefines.PP_ICL_PTCACHE_SIZE;
            }
        }

        /// <summary>
        /// Constructs a new VertexCacheSizeConfig.
        /// </summary>
        /// <param name="vertexCacheSize">Size of the post-transform vertex cache, in number of vertices.</param>
        public VertexCacheSizeConfig(int vertexCacheSize) 
            : base(VertexCacheSizeConfigName, vertexCacheSize, VertexCacheSizeConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.RemoveComponent"/> step that
    /// specifies which parts of the data structure is to be removed. If no valid mesh
    /// remains after the step, the import fails. The default value i <see cref="ExcludeComponent.None"/>.
    /// </summary>
    public class RemoveComponentConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by RemoveComponentConfig.
        /// </summary>
        public static String RemoveComponentConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_RVC_FLAGS;
            }
        }

        /// <summary>
        /// Constructs a new RemoveComponentConfig.
        /// </summary>
        /// <param name="componentsToExclude">Bit-wise combination of components to exclude.</param>
        public RemoveComponentConfig(ExcludeComponent componentsToExclude) 
            : base(RemoveComponentConfigName, (int) componentsToExclude, (int) ExcludeComponent.None) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.SortByPrimitiveType"/> step that
    /// specifies which primitive types are to be removed by the step. Specifying all
    /// primitive types is illegal. The default value is zero specifying none.
    /// </summary>
    public class SortByPrimitiveTypeConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by SortByPrimitiveTypeConfig.
        /// </summary>
        public static String SortByPrimitiveTypeConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SBP_REMOVE;
            }
        }

        /// <summary>
        /// Constructs a new SortByPrimitiveTypeConfig.
        /// </summary>
        /// <param name="typesToRemove">Bit-wise combination of primitive types to remove</param>
        public SortByPrimitiveTypeConfig(PrimitiveType typesToRemove)
            : base(SortByPrimitiveTypeConfigName, (int) typesToRemove, 0) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.FindInvalidData"/> step that
    /// specifies the floating point accuracy for animation values, specifically
    /// the episilon during comparisons. The default value is 0.0f.
    /// </summary>
    public class AnimationAccuracyConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by AnimationAccuracyConfig.
        /// </summary>
        public static String AnimationAccuracyConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_FID_ANIM_ACCURACY;
            }
        }

        /// <summary>
        /// Constructs a new AnimationAccuracyConfig.
        /// </summary>
        /// <param name="episilon">Episilon for animation value comparisons.</param>
        public AnimationAccuracyConfig(float episilon) 
            : base(AnimationAccuracyConfigName, episilon, 0.0f) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.TransformUVCoords"/> step that
    /// specifies which UV transformations are to be evaluated. The default value
    /// is for all combinations (scaling, rotation, translation).
    /// </summary>
    public class TransformUVConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by TransformUVConfig.
        /// </summary>
        public static String TransformUVConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_TUV_EVALUATE;
            }
        }

        /// <summary>
        /// Constructs a new TransformUVConfig.
        /// </summary>
        /// <param name="transformFlags">Bit-wise combination specifying which UV transforms that should be evaluated.</param>
        public TransformUVConfig(UVTransformFlags transformFlags)
            : base(TransformUVConfigName, (int) transformFlags, (int) AiDefines.AI_UVTRAFO_ALL) { }
    }

    /// <summary>
    /// Configuration that is a hint to Assimp to favor speed against import quality. Enabling this
    /// option may result in faster loading, or it may not. It is just a hint to loaders
    /// and post-process steps to use faster code paths if possible. The default value is false.
    /// </summary>
    public class FavorSpeedConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by FavorSpeedConfig.
        /// </summary>
        public static String FavorSpeedConfigName {
            get {
                return AiConfigs.AI_CONFIG_FAVOUR_SPEED;
            }
        }

        /// <summary>
        /// Constructs a new FavorSpeedConfig.
        /// </summary>
        /// <param name="favorSpeed">True if Assimp should favor speed at the expense of quality, false otherwise.</param>
        public FavorSpeedConfig(bool favorSpeed) 
            : base(FavorSpeedConfigName, favorSpeed, false) { }
    }

    #endregion
}
