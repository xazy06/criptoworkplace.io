module.exports = function (grunt) {
	grunt.initConfig({
		concat: {
			build: {
				src: ['wwwroot/js/*'],
				dest: 'wwwroot/js/app.js'
			}
		},
		uglify: {
			build: {
				files: {
					'wwwroot/bundles/jquery.js': ['wwwroot/lib/jquery/dist/jquery.js'],
					'wwwroot/bundles/jqueryval.js': ['wwwroot/lib/jquery-validation/dist/jquery.validate.js', 'lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js'],
					'wwwroot/bundles/bootstrap.js': ['wwwroot/lib/bootstrap/dist/js/bootstrap.min.js'],
					'wwwroot/bundles/app.js': ['wwwroot/js/app.js']
				}
			}
		},
		clean: ["wwwroot/js/app.js"],
		cssmin: {
			build: {
				files: {
					'wwwroot/bundles/main.css': ['wwwroot/lib/bootstrap/dist/css/bootstrap.css', 'wwwroot/css/site.css']
				}
			}
		},
		copy: {
			build: {
				expand: true,
				cwd: 'wwwroot/lib/bootstrap/dist/fonts/',
				src: '**',
				dest: 'wwwroot/fonts/',
				flatten: true,
				filter: 'isFile'
			}
		},"default": {
			"files": [
				{
					"expand": true,
					"src": [ "Sass/*.scss" ],
					"dest": "wwwroot/css", // or "<%= src %>" for output to the same (source) folder
					"ext": ".css"
				}
			]
		},
		"watch": {
			"sass": {
				"files": [ "Sass/*.scss" ],
				"tasks": [ "sass" ],
				"options": {
					"livereload": true
				}
			}
		}
	});

	// grunt.loadNpmTasks('grunt-contrib-clean');
	// grunt.loadNpmTasks('grunt-contrib-copy');
	// grunt.loadNpmTasks('grunt-contrib-cssmin');
	// grunt.loadNpmTasks('grunt-contrib-concat');
	// grunt.loadNpmTasks('grunt-contrib-uglify');

	grunt.loadNpmTasks("grunt-bower-task");
	grunt.loadNpmTasks("grunt-contrib-watch");
	grunt.loadNpmTasks("grunt-contrib-sass");

	grunt.registerTask('build', ['clean', 'copy', 'concat', 'uglify', 'cssmin', 'watch']);
};