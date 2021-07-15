shader_type spatial;
render_mode depth_draw_alpha_prepass,specular_disabled,unshaded,shadows_disabled,skip_vertex_transform;

uniform vec3 light_dir = vec3(0.0, 1.0, -1.0);

uniform vec3 cloud_cube_size = vec3(10.0, 10.0, 10.0);
uniform float cloud_density_factor = 5.0;
uniform int cloud_steps = 128;

varying vec3 pos;
varying vec3 dir;
varying vec3 ldir;

void vertex() {
	// get our pos on the surface of our mesh in model space
	pos = VERTEX;

	// make our vertex position
	VERTEX = (MODELVIEW_MATRIX * vec4(VERTEX, 1.0)).xyz;

	// get our direction for our raymarch
	dir = (inverse(MODELVIEW_MATRIX) * vec4(normalize(VERTEX), 0.0)).xyz;

	// and our light dir
	ldir = (inverse(WORLD_MATRIX) * vec4(normalize(light_dir),0.0)).xyz;
}

float N31(in vec3 p) {
	p = fract(p * vec3(0.1031, 0.11369, 0.13787));
	p += dot(p, p.yzx + 19.19);
	return -1.0 + 2.0 * fract((p.x + p.y) * p.z);
}

float valueNoise(vec3 p) {
	vec3 pi = floor(p);
	vec3 pf = fract(p);

	vec3 w = pf * pf * (3.0f - 2.0f * pf);

	return mix(
		mix(
			mix(N31(pi + vec3(0, 0, 0)), N31(pi + vec3(1, 0, 0)), w.x),
			mix(N31(pi + vec3(0, 0, 1)), N31(pi + vec3(1, 0, 1)), w.x),
			w.z
		),
		mix(
			mix(N31(pi + vec3(0, 1, 0)), N31(pi + vec3(1, 1, 0)), w.x),
			mix(N31(pi + vec3(0, 1, 1)), N31(pi + vec3(1, 1, 1)), w.x),
			w.z
		),
		w.y
	);
}

//float cloud_density(in vec3 p) {
//	p = smoothstep(0.45, 0.55, fract(p / 5.0));
//	float value = mod(mod(p.x + p.y, 2.0) + p.z, 2.0) * 0.5;
//	return value;
//}

float cloud_density(in vec3 p) {
	float noise = (valueNoise(p * 0.7) + 1.0f) * 0.5f;
	float value = smoothstep(-0.4, 0.4, (1.0 - sqrt(p.x * 0.2 * p.x * 0.2 + p.y * 0.3 * p.y * 0.3 + p.z * 0.2 * p.z * 0.2))) * noise * 0.25;
	return value;
}

void fragment() {
	vec3 color = vec3(0.0, 0.0, 0.0);
	float alpha = 0.0;
	float transmittance = 1.0;
	float cdf = cloud_density_factor / float(cloud_steps);

	// lets raymarch...
	vec3 p = pos;
	vec3 s = cloud_cube_size * cloud_cube_size;
	float diag = sqrt(s.x + s.y + s.z);
	vec3 d = diag * normalize(dir) / float(cloud_steps);

	for(int i = 0; i < int(cloud_steps); i++) {
		float density = clamp(cloud_density(p), 0.0, 1.0) * cdf;
		
		if (density > 0.001) {
			color += density * transmittance;
			alpha += density;
			transmittance *= 1.0 - density;

			if (alpha >= 1.0) {
				alpha = 1.0;
				break;
			}
		}

		// move through our cube
		p += d;

		// still within our cube?
		s = cloud_cube_size / 2.0;
		if (p.x < -s.x || p.x > s.x || p.y < -s.y || p.y > s.y || p.z < -s.z || p.z > s.z) {
			break;
		}
	}

	if (alpha > 0.01) {
		// alpha will be applied to our color so reverse apply it
		color = color / alpha;
	}

	ALBEDO = clamp(color, 0.0, 1.0);
	ALPHA = alpha;
}
