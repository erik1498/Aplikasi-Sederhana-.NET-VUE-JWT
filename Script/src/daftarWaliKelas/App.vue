<template>
    <div class="row">
        <div class="col-12">
            <h4>{{ title }}</h4>
        </div>
    </div>
    <div v-if="datashow" class="row">
        <div class="col-3 offset-9">
            <ButtonTable @click="tambahToggle" class="btn btn-primary btn-block" text="Tambah"/>
        </div>
    </div>
    <div v-if="datashow" class="row my-3">
        <div class="col-12">
            <div class="form-group">
                <div class="input-group mb-3">
                    <input type="text" class="form-control" v-model="searchKey" placeholder="Cari Wali Kelas" aria-label="Cari Wali Kelas" aria-describedby="cariWaliKelas">
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary" @click="cariWaliKelas" type="button">Cari</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-if="datashow" class="row">
        <div class="col-lg-12">
            <table class="table table-sm">
                <thead>
                    <tr>
                        <th>Nama Wali Kelas</th>
                        <th>Nama Kelas</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in walikelas" key="item.WaliKelasID">
                        <td>{{ item.NamaWaliKelas }}</td>
                        <td>{{ item.NamaKelas }}</td>
                        <td>
                            <ButtonTable @click="editToggle(item.WaliKelasID)" class="btn btn-warning btn-sm mx-1" text="Edit"/>
                            <ButtonTable @click="hapusWaliKelas(item.WaliKelasID)" class="btn btn-danger btn-sm mx-1" text="Hapus"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div v-if="!datashow">
            <div class="row">
                <div class="col-12 mt-3">
                    <div class="form-group" v-if="kelas.length > 0">
                        <label for="NamaWaliKelas">Nama Wali Kelas</label>
                        <input type="text" class="form-control" v-model="NamaWaliKelas" autocomplete="off">
                        <p v-if="Error['NamaWaliKelas']" class="text-danger">* {{ Error['NamaWaliKelas'] }}</p>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label for="NamaWaliKelas">Kelas Bimbingan</label>
                        <select v-model="KelasID" class="form-control" v-if="kelas.length > 0">
                            <option v-for="item in kelas" key="item.KelasID" :value="item.KelasID">
                                {{ item.NamaKelas }}
                            </option>
                        </select>
                        <p class="text-danger" v-if="kelas.length == 0">* Kelas Tidak Tersedia</p>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group" v-if="kelas.length > 0">
                        <label for="Username">Username</label>
                        <input type="text" class="form-control" v-model="Username" autocomplete="off">
                        <p v-if="Error['Username']" class="text-danger">* {{ Error['Username'] }}</p>
                    </div>
                </div>
                <div class="col-6">
                    <div class="form-group" v-if="kelas.length > 0">
                        <label for="Password">Password</label>
                        <input type="password" class="form-control" v-model="Password" autocomplete="off">
                        <p v-if="Error['Password']" class="text-danger">* {{ Error['Password'] }}</p>
                    </div>
                </div>
                <div class="col-6">
                    <ButtonTable @click="tambahToggle" text="Batal" class="btn btn-block btn-danger" />
                </div>
                <div class="col-6">
                    <ButtonTable @click="simpanWaliKelas"  v-if="kelas.length > 0" text="Simpan" class="btn btn-block btn-success" />
                </div>
            </div>
    </div>
</template>

<script>
import querystring from 'querystring';
import axios from 'axios';
import baseWaliKelas from './apiVariable';
import ButtonTable from './../components/ButtonTable.vue'

export default {
    name:'App',
    data(){
        return {
            walikelas:[],
            kelas:[],
            KelasID:0,
            token:token,
            title:"Daftar Wali Kelas",
            datashow:true,
            NamaWaliKelas:"",
            WaliKelasID:0,
            searchKey:"",
            Error:{},
            Username:"",
            Password:"",
        }
    },
    components:{
        ButtonTable
    },
    methods:{
        tambahToggle(){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Wali Kelas" : "Tambah Wali Kelas"
            this.NamaWaliKelas = ""
            this.WaliKelasID = 0
            this.Username = ""
            this.Password = ""
            this.Error = {}
            if (!this.datashow) {
                this.getKelasTanpaWaliKelas()
            }
        },
        editToggle(idWaliKelasSelect){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Wali Kelas" : "Edit Wali Kelas"
            this.Error = {}
            this.getKelasTanpaWaliKelas();
            axios.get(baseWaliKelas(`GetWaliKelas/${idWaliKelasSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            }).then(res => {
                this.NamaWaliKelas = res.data.NamaWaliKelas
                this.WaliKelasID = res.data.WaliKelasID
                this.kelas.push({
                    NamaKelas : res.data.NamaKelas,
                    KelasID : res.data.KelasID,
                })
                this.KelasID = res.data.KelasID
            })
        },
        getKelasTanpaWaliKelas(){
            axios.get(baseWaliKelas("GetKelasTanpaWaliKelas"), {
                headers:{
                    "Authorization" : `Bearer ${this.token}`
                }
            }).then(res => {
                this.kelas = res.data;
                this.KelasID = this.kelas[0].KelasID
            })
        },
        hapusWaliKelas(idWaliKelasSelect){
            axios.delete(baseWaliKelas(`Delete/${idWaliKelasSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            })
            this.walikelas.forEach((item, index) => {
                if (item.WaliKelasID == idWaliKelasSelect) {
                    this.walikelas.splice(index, 1);
                    return
                }
            })
        },
        simpanWaliKelas(){
            let data = {
                WaliKelasID : this.WaliKelasID,
                NamaWaliKelas: this.NamaWaliKelas,
                KelasID: this.KelasID,
                Username: this.Username,
                Password: this.Password
            }
            this.title = "Daftar Wali Kelas"
            if (data.WaliKelasID > 0) {
                axios.put(baseWaliKelas(`Edit`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.changeWaliKelas(res);
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }else{
                axios.post(baseWaliKelas(`Create`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.addWaliKelas(res, this.kelas.filter((item) => {
                        return item.KelasID == this.KelasID
                    })[0].NamaKelas)
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }
        },
        cariWaliKelas(){
            let data = {
                NamaWaliKelas: this.searchKey
            }
            if (data.NamaWaliKelas.length > 0) {
                axios.post(baseWaliKelas(`Search?area=Wali Kelas`), querystring.stringify(data)).then((res) => {
                    this.walikelas = res.data
                }, {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).catch((res) => {
                    if (res.response.data) {
                        console.log(res.response)
                    }
                })
            }else{
                axios.get(baseWaliKelas('GetDaftarWaliKelas'), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.walikelas = res.data
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
        changeWaliKelas(res){
            this.walikelas.forEach((item, index) => {
                if (item.WaliKelasID == res.data.WaliKelasID) {
                    this.walikelas[index].NamaWaliKelas = res.data.NamaWaliKelas
                    this.walikelas[index].NamaKelas = res.data.NamaKelas
                    return
                }
            })
        },
        addWaliKelas(res, NamaKelas){
            this.walikelas.push({
                NamaWaliKelas:res.data.NamaWaliKelas,
                WaliKelasID:res.data.WaliKelasID,
                NamaKelas:NamaKelas
            })
        }
    },
    created(){
        axios.get(baseWaliKelas('GetDaftarWaliKelas'),{
            headers:{
                "Authorization" : `Bearer ${token}`
            }
        }).then(res => {
            this.walikelas = res.data
        })
    }
}
</script>

<style>

</style>