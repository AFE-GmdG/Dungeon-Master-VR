shader_type spatial;
render_mode specular_schlick_ggx;

uniform sampler2D albedoTex : hint_albedo;
uniform sampler2D bumpTex : hint_normal;
uniform float bumpStrength: hint_range(0.0, 1.0, 0.01) = 0.5;

void fragment() {
	vec3 bumpVec3 = texture(bumpTex, UV.xy).rgb;
    bumpVec3.y = 1.0 - bumpVec3.y;

	ALBEDO = texture(albedoTex, UV.xy).rgb;
	NORMALMAP = bumpVec3;
	NORMALMAP_DEPTH = bumpVec3.z * bumpStrength;
}
