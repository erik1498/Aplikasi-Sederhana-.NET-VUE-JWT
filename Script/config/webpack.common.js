const path = require('path')
const { VueLoaderPlugin } = require('vue-loader')

const __base = path.resolve(__dirname, '..');
const __srcDaftarSiswa = path.resolve(__base, 'src/daftarSiswa');
const __srcDaftarWaliKelas = path.resolve(__base, 'src/daftarWaliKelas');
const __srcDaftarKelas = path.resolve(__base, 'src/daftarKelas');
const __srcDaftarJurusan = path.resolve(__base, 'src/daftarJurusan');
const __srcHome = path.resolve(__base, 'src/home');

module.exports = {
    entry:{
        daftarSiswa:path.resolve(__srcDaftarSiswa, 'index.js'),
        daftarWaliKelas:path.resolve(__srcDaftarWaliKelas, 'index.js'),
        daftarKelas:path.resolve(__srcDaftarKelas, 'index.js'),
        daftarJurusan:path.resolve(__srcDaftarJurusan, 'index.js'),
        home:path.resolve(__srcHome, 'index.js'),
    },
    plugins:[
        new VueLoaderPlugin(),
    ],
    module:{
        rules:[
            {
                test:/\.vue$/,
                loader:'vue-loader'
            },
            {
                test:/\.(png|svg|jpg|jpeg)$/,
                type:'asset/resource'
            },
            {
                test:/\.js$/,
                exclude:/node_modules/,
                use:{
                    loader: 'babel-loader',
                    options:{
                        presets:['@babel/preset-env']
                    }
                }
            }
        ]
    },
    output:{
        filename:'[name]/[name].js',
        path:path.resolve(__base,'..','wwwroot','bundle'),
        clean:true,
    }
}