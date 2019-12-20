#version 330 core
layout (points) in;
layout (triangle_strip, max_vertices = 3) out;

in VS_OUT {
    vec3 color;
} gs_in[];

out vec3 color;

void build_house(vec4 position)
{
    color = gs_in[0].color;
    gl_Position = position + (-0.2, -0.2, 0.0, 0.0); // bottom-left
    EmitVertex();

    gl_Position = position + (0.2, -0.2, 0.0, 0.0); // bottom-right
    EmitVertex();

    gl_Position = position + (-0.2, 0.2, 0.0, 0.0); // top-left
    EmitVertex();

    gl_Position = position + (0.2, 0.2, 0.0, 0.0); // top-right
    EmitVertex();

    gl_Position = position + (0.0, 0.4, 0.0, 0.0); // top
    color = vec3(1.0, 1.0, 1.0);
    EmitVertex();

    EndPrimitive();
}


void main() {    
    gl_Position = gl_in[0].gl_Position; 
    color = gs_in[0].color;
    EmitVertex();

    gl_Position = gl_in[0].gl_Position + vec4(0.4, 0.0, 0.0, 0.0);
    EmitVertex();

    gl_Position = gl_in[0].gl_Position + vec4(0.0, 0.2, 0.0, 0.0);
    EmitVertex();

    EndPrimitive();
}  