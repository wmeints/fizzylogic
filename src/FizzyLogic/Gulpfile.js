var gulp = require('gulp');
var sass = require('gulp-sass');

exports.compileStylesheets = function compileStylesheets() {
    return gulp.src('./Stylesheets/site.scss').pipe(sass({ outputStyle: 'compressed' })).pipe(gulp.dest('wwwroot/css'));
};

exports.copyJavascriptFiles = function copyJavascriptFiles() {
    return gulp.src([
        'node_modules/bootstrap/dist/js/bootstrap.bundle.js',
        'node_modules/infinite-scroll/dist/infinite-scroll.pkgd.min.js',
        'node_modules/jquery/dist/jquery.min.js'
    ]).pipe(gulp.dest('wwwroot/js'));
};

exports.default = gulp.series([exports.compileStylesheets, exports.copyJavascriptFiles]);