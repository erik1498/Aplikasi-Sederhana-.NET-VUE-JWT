const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const CssMinimizerWebpackPlugin = require('css-minimizer-webpack-plugin')

const { merge } = require('webpack-merge')
const common = require('./webpack.common.js')

module.exports = merge(common, {
    mode:'production',
    devtool:false,
    plugins:[
        new MiniCssExtractPlugin()
    ],
    module:{
        rules:[
            {
                test:/\.css$/,
                use:[
                    MiniCssExtractPlugin.loader, 'css-loader',
                ]
            },
        ]
    },
    optimization:{
        minimize:true,
        minimizer: [
            '...',
            new CssMinimizerWebpackPlugin(),
        ]
    }
})