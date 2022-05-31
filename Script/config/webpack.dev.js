const { merge } = require('webpack-merge')
const common = require('./webpack.common.js')

module.exports = merge(common, {
    mode:'development',
    devtool:'inline-sorce-map',
    devServer:{
        static:'./dist',
        open:true,
        hot:true
    },
    module:{
        rules:[
            {
                test:/\.css$/,
                use:[
                    'vue-style-loader',
                    'css-loader'
                ]
            },
        ],
    },
})