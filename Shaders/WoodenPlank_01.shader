shader_type spatial;
render_mode specular_schlick_ggx;

uniform sampler2D diffuseTexture: hint_albedo;
uniform sampler2D normalTexture: hint_normal;

void fragment() {
    vec4 diffuse = texture(diffuseTexture, UV.xy);
    vec4 normal = texture(normalTexture, UV.xy);
    normal.y = 1.0 - normal.y;
    ALBEDO = diffuse.rgb;
    NORMALMAP = normal.xyz;
}
