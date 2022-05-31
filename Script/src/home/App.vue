<template>
    <div class="row">
        <div class="col-12">
            <h4>Login</h4>
        </div>
    </div>
    <div class="row mt-5">
        <div class="col-lg-6">
            <div class="form-group">
                <label>Username</label>
                <input type="text" class="form-control" v-model="username">
                <p v-if="Error['username']" class="text-danger">* {{ Error['username'] }}</p>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="form-group">
                <label>Password</label>
                <input type="password" class="form-control" v-model="password">
                <p v-if="Error['password']" class="text-danger">* {{ Error['password'] }}</p>
            </div>
        </div>
        <div class="col-12">
            <p v-if="Error['loginMessage']" class="text-danger">* {{ Error['loginMessage'] }}</p>
            <ButtonTable @click="login" text="Login" class="btn btn-primary btn-block"/>
        </div>
    </div>
</template>

<script>
import querystring from 'querystring';
import axios from 'axios';
import baseHome from './apiVariable';
import ButtonTable from './../components/ButtonTable.vue'

export default {
    name:'App',
    data(){
        return {
            username:"",
            password:"",
            Error:[]
        }
    },
    components:{
        ButtonTable
    },
    methods:{
        login(){
            let data = {
                username : this.username,
                password : this.password,
            }
            this.Error = []
            if (data.username == "" || data.password == "") {
                if (data.username == "") {
                    this.createErrorClientSide("username", "Tidak boleh kosong");
                }
                if (data.password == "") {
                    this.createErrorClientSide("password", "Tidak boleh kosong");
                }
            }else{
                axios.post(baseHome('Login'), querystring.stringify(data)).then(res => {
                    if(res.status == 200){
                        let formLogin = document.querySelector('form');
                        let usernameField = document.querySelector('input[name="Username"]');
                        let passwordField = document.querySelector('input[name="Password"]');

                        usernameField.value = this.username
                        passwordField.value = this.password
                        formLogin.submit()
                    }
                }).catch((error) => {
                    if (error.response.status == 404) {
                        this.Error["loginMessage"] = "Username atau Password anda salah !!!"
                    }else{
                        this.createError(error);
                    }
                })
            }
        },
        createError(error){
            let errorMessage = Object.values(error.response.data)
            errorMessage.forEach(element => {
                if (element.Errors.length > 0) {
                    this.Error[element.Key] = element.Errors[0].ErrorMessage
                }
            });
        },
        createErrorClientSide(key, message){
            this.Error[key] = message
        }
    }
}
</script>

<style>

</style>