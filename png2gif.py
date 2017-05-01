#!/usr/bin/env python

# Convert PNG images to GIF, preserving transparency
# 2008 - http://www.coderholic.com/png2gif

import sys
import Image
import random
import optparse
import os
import os.path

def unique_color(image):
	"""find a color that doesn't exist in the image
	"""
	colors = image.getdata()
	while True:
		# Generate a random color
		if image.mode == "LA":
			 color = random.randint(0, 255),
		else:
			color = (
			  random.randint(0, 255),
			  random.randint(0, 255),
			  random.randint(0, 255)
			)

		if color not in colors:
			return color

def fill_transparent(image, color, threshold=0):
	"""Fill transparent image parts with the specified color
	"""
	def quantize_and_invert(alpha):
		if alpha <= threshold:
			return 255
		return 0
	# Get the alpha band from the image
	if image.mode == 'RGBA':
		red, green, blue, alpha = image.split()
	elif image.mode == 'LA':
		gray, alpha = image.split()
	# Set all pixel values below the given threshold to 255,
	# and the rest to 0
	alpha = Image.eval(alpha, quantize_and_invert)
	# Paste the color into the image using alpha as a mask
	image.paste(color, alpha)


def color_index(image, color):
	"""Find the color index
	"""
	palette = image.getpalette()
	palette_colors = zip(palette[::3], palette[1::3], palette[2::3])
	index = palette_colors.index(color)
	return index

def convert_image(image, name, new_name):
	if image.mode == 'P':
		if image.info.has_key('transparency'): # check to see if the image has any transparency
			transparency = image.info['transparency']
			image.save(new_name, transparency=transparency)
		else: image.save(new_name)
	elif image.mode == 'RGBA': # RGB images need to be converted to Palette mode
		threshold = 0
		colour = unique_color(image)
		fill_transparent(image, colour, threshold)
		image = image.convert('RGB').convert('P', palette=Image.ADAPTIVE)
		image.save(new_name, transparency=color_index(image, colour))
	elif image.mode == 'LA':
		threshold = 0
		colour = unique_color(image)
		fill_transparent(image, colour, threshold)
		image = image.convert('L').convert('P', palette=Image.ADAPTIVE)
		image.save(new_name, transparency=color_index(image, (colour[0], colour[0], colour[0])))
	else:
		raise "Unsupported PNG file" + name + "(" + image.mode + ")"

p = optparse.OptionParser(description = 'Convert PNG images to GIF format',
						  usage = 'png2gif [OPTIONS] &lt;files&gt;')
p.add_option('--outputdir', '-o', default='.', help='Set the output directory in which to put the GIF images. Defaults to the current directory')
p.add_option('--threshold', '-t', default='0', help='Set the transparency threshold. Defaults to 0')
p.add_option('--replace', '-r', action='store_true', help='Delete the PNG files after converting them to GIF')
p.add_option('--verbose', '-v', action='store_true', help='Verbose output')

options, arguments = p.parse_args()

# Check that the specified output directory exists
if not os.path.exists(options.outputdir):
	if options.versbose: print "Creating output directory", options.outputdir
	try:
		os.mkdir(options.outputdir)
	except:
		print "Cannot create output directory", options.outputdir
		print "Please specifiy a different directory"
		exit

for i in range(1, len(arguments)):
	# Make sure we have the correct extension
	extension = arguments[i][-3:]
	if extension != "png":
		print "Invalid file extension for argument ", i-1, ". Expecting png found ", extension
		continue
	image = Image.open(arguments[i])
	if options.verbose:
		print "Converting", arguments[i], "to", arguments[i][:-3] + "gif"
	try:
		convert_image(image, arguments[i], options.outputdir +os.sep + arguments[i][:-3] + "gif")
		if options.replace:
			if options.verbose: print "Removing", arguments[i]
			os.remove(arguments[i])
	except:
		print "Problem converting",arguments[i], "- skipping"
