
camera{
	location <0, 4.83, -7.635>
	angle 29.1
	right x*image_width/image_height
	up y
	rotate <0, 0, 0>
}

light_source{
    #declare intensity = 1;
    <2.513, 6.12225, -3.047> rgb <intensity, intensity, intensity>
    #declare resolution = 5;
    area_light <-0.1664684, 0.9317747, 0.3226207>, <0.8886717, -5.215406E-08, 0.458544>, resolution, resolution
    adaptive 1
    jitter
}

light_source{
    #declare intensity = 1;
    <-3.142, 6.12225, -3.047> rgb <intensity, intensity, intensity>
    #declare resolution = 5;
    area_light <0.1450303, 0.9317747, 0.3328095>, <0.9167368, -3.72529E-08, -0.3994917>, resolution, resolution
    adaptive 1
    jitter
}

light_source{
    #declare intensity = 1;
    <0, 6.45, 1.99> rgb <intensity, intensity, intensity>
    fade_distance 10
    fade_power 1
}



