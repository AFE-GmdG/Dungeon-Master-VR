shader_type spatial;
render_mode unshaded, cull_disabled;

uniform int NUMBER_OF_STEPS = 32;
//varying vec3 camera_origin;

// params:
// p: arbitrary point in 3D space
// c: the center of our sphere
// r: the radius of our sphere
float distance_from_sphere(in vec3 p, in vec3 c, float r)
{
    return length(p - c) - r;
}

float map_the_world(in vec3 p, float time)
{
        //float displacement = sin(5.0 * p.x) * sin(5.0 * p.y) * sin(5.0 * p.z) * 0.25;
        //displacement *= cos(time);
    float sphere_0 = distance_from_sphere(p, vec3(0.0), 0.5);

    // Later we might have sphere_1, sphere_2, cube_3, etc...

//    return sphere_0 + displacement;
    return sphere_0;
}

vec3 calculate_normal(in vec3 p, float time)
{
    const vec3 small_step = vec3(0.001, 0.0, 0.0);

    float gradient_x = map_the_world(p + small_step.xyy, time) - map_the_world(p - small_step.xyy, time);
    float gradient_y = map_the_world(p + small_step.yxy, time) - map_the_world(p - small_step.yxy, time);
    float gradient_z = map_the_world(p + small_step.yyx, time) - map_the_world(p - small_step.yyx, time);

    vec3 normal = vec3(gradient_x, gradient_y, gradient_z);

    return normalize(normal);
}

vec3 ray_march(in vec3 ro, in vec3 rd, float time, out bool hit)
{
	float total_distance_traveled = 0.0;
	const float MINIMUM_HIT_DISTANCE = 0.001;
	const float MAXIMUM_TRACE_DISTANCE = 1000.0;

	for (int i = 0; i < NUMBER_OF_STEPS; ++i)
	{
		vec3 current_position = ro + total_distance_traveled * rd;

		float distance_to_closest = map_the_world(current_position, time);

		if (distance_to_closest < MINIMUM_HIT_DISTANCE) // hit
		{
			// Let's return a diffuse from a fake light
			vec3 normal = calculate_normal(current_position, time);

			// For now, hard-code the light's position in our scene

			vec3 light_position = vec3(2.0, -5.0, 3.0);

			// Calculate the unit direction vector that points from
			// the point of intersection to the light source
			vec3 direction_to_light = normalize(current_position - light_position);

			float diffuse_intensity = max(0.0, dot(normal, direction_to_light));
			hit = true;
			return vec3(1.0, 0.0, 0.0) * diffuse_intensity;
		}

		if (total_distance_traveled > MAXIMUM_TRACE_DISTANCE) // miss
		{
			break;
		}

		// accumulate the distance traveled thus far
		total_distance_traveled += distance_to_closest;
	}

	// If we get here, we didn't hit anything so just
	// return a background color (black)
	hit = false;
	return vec3(0.0);
}

void fragment() {
    // Get Cameran origin
    vec3 ro = (CAMERA_MATRIX * vec4(0.0, 0.0, 0.0, 1.0)).xyz;

		// Get fragment position in world space coordinates
		vec3 p = ((CAMERA_MATRIX * vec4(VERTEX, 1.0)).xyz);

    // Get Ray direction (From camera to vertex)
    vec3 rd = normalize(p - ro);

    bool hit;
    vec3 color = ray_march(ro, rd, TIME, hit);

    if (hit) {
        ALBEDO = color;
        ALPHA = 1.0;
    }
    else {
        ALPHA = 0.0;
    }
}
