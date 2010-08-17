version = File.read(File.expand_path("../VERSION",  __FILE__)).strip

Gem::Specification.new do |spec|
	spec.platform	= Gem::Platform::RUBY
	spec.name	= 'dotless'
	spec.version 	= version
	spec.files 	= Dir['lib/**/*']
	spec.bindir	= 'bin'
	spec.executables << 'dotless'

	spec.summary	= 'dotless - dynamic CSS for .NET'
	spec.description = 'dotless lets you write dynamic CSS leveraging the .less language'
	
	spec.authors	= ['Daniel "Tigraine" Hoelbling', 'James Foster']
	spec.email	= 'dotless@googlegroups.com'
	
	spec.homepage 	= 'http://github.com/dotless/dotless'
	spec.rubyforge_project = 'dotless'
end
