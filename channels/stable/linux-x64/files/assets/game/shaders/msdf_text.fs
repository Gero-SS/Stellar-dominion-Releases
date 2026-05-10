#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;
uniform float u_distanceRange;
uniform vec2 u_atlasSize;

out vec4 finalColor;

float median(float r, float g, float b) {
  return max(min(r, g), min(max(r, g), b));
}

float screenPxRange() {
  vec2 unitRange = vec2(u_distanceRange) / u_atlasSize;
  vec2 screenTexSize = vec2(1.0) / fwidth(fragTexCoord);
  return max(0.5 * dot(unitRange, screenTexSize), 1.0);
}

void main() {
  vec3 msd = texture(texture0, fragTexCoord).rgb;
  float sd = median(msd.r, msd.g, msd.b);
  float screenDistance = screenPxRange() * (sd - 0.5);
  float opacity = clamp(screenDistance + 0.5, 0.0, 1.0);
  finalColor = vec4(fragColor.rgb * colDiffuse.rgb, fragColor.a * colDiffuse.a * opacity);
}
