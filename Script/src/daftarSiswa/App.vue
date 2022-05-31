<template>
    <div class="row">
        <div class="col-12">
            <h4>{{ title }}</h4>
        </div>
    </div>
    <div v-if="datashow" class="row">
        <div class="col-3 offset-9">
            <ButtonTable @click="tambahToggle" class="btn btn-primary btn-block" v-if="role == 1" text="Tambah"/>
        </div>
    </div>
    <div v-if="datashow" class="row my-3">
        <div class="col-12">
            <div class="form-group">
                <div class="input-group mb-3">
                    <input type="text" class="form-control" v-model="searchKey" placeholder="Cari Siswa" aria-label="Cari Siswa" aria-describedby="cariSiswa">
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary" @click="cariSiswa" type="button">Cari</button>
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
                        <th>Nama Siswa</th>
                        <th v-if="role == 1"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in siswa" key="item.SiswaID">
                        <td>{{ item.NamaSiswa }}</td>
                        <td v-if="role == 1">
                            <ButtonTable @click="editToggle(item.SiswaID)" class="btn btn-warning btn-sm mx-1" text="Edit"/>
                            <ButtonTable @click="hapusSiswa(item.SiswaID)" class="btn btn-danger btn-sm mx-1" text="Hapus"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div v-if="!datashow">
            <div class="row">
                <div class="col-12 my-3">
                    <div class="form-group">
                        <label for="NamaSiswa">Nama Siswa</label>
                        <input type="text" class="form-control" v-model="NamaSiswa" autocomplete="off">
                        <p v-if="Error['NamaSiswa']" class="text-danger">* {{ Error['NamaSiswa'] }}</p>
                    </div>
                </div>
                <div class="col-6">
                    <ButtonTable @click="tambahToggle" text="Batal" class="btn btn-block btn-danger" />
                </div>
                <div class="col-6">
                    <ButtonTable @click="simpanSiswa" text="Simpan" class="btn btn-block btn-success" />
                </div>
            </div>
    </div>
</template>

<script>
import querystring from 'querystring';
import axios from 'axios';
import baseSiswa from './apiVariable';
import ButtonTable from './../components/ButtonTable.vue'

export default {
    name:'App',
    data(){
        return {
            siswa:[],
            token:token,
            title:"Daftar Siswa",
            datashow:true,
            NamaSiswa:"",
            SiswaID:0,
            searchKey:"",
            role:role,
            Error:{}
        }
    },
    components:{
        ButtonTable
    },
    methods:{
        tambahToggle(){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Siswa" : "Tambah Siswa"
            this.NamaSiswa = ""
            this.SiswaID = 0
            this.Error = {}
        },
        editToggle(idSiswaSelect){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Siswa" : "Edit Siswa"
            this.Error = {}
            axios.get(baseSiswa(`GetSiswa/${idSiswaSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            }).then(res => {
                this.NamaSiswa = res.data.NamaSiswa
                this.SiswaID = res.data.SiswaID
            })
        },
        hapusSiswa(idSiswaSelect){
            axios.delete(baseSiswa(`Delete/${idSiswaSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            })
            this.siswa.forEach((item, index) => {
                if (item.SiswaID == idSiswaSelect) {
                    this.siswa.splice(index, 1);
                    return
                }
            })
        },
        simpanSiswa(){
            let data = {
                SiswaID : this.SiswaID,
                NamaSiswa: this.NamaSiswa
            }
            this.title = "Daftar Siswa"
            if (data.SiswaID > 0) {
                axios.put(baseSiswa(`Edit?area=Siswa`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.changeSiswa(res);
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }else{
                axios.post(baseSiswa(`Create?area=Siswa`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                   this.addSiswa(res)
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }
        },
        cariSiswa(){
            let data = {
                NamaSiswa: this.searchKey
            }
            if (data.NamaSiswa.length > 0) {
                axios.post(baseSiswa(`Search?area=Siswa`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then((res) => {
                    this.siswa = res.data
                }).catch((res) => {
                    if (res.response.data) {
                        console.log(res.response)
                    }
                })
            }else{
                axios.get(baseSiswa('GetDaftarSiswa'), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.siswa = res.data
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
        changeSiswa(res){
            this.siswa.forEach((item, index) => {
                if (item.SiswaID == res.data.siswaID) {
                    this.siswa[index].NamaSiswa = res.data.namaSiswa
                    return
                }
            })
        },
        addSiswa(res){
            this.siswa.push({
                NamaSiswa:res.data.NamaSiswa,
                SiswaID:res.data.SiswaID
            })
        }
    },
    created(){
        axios.get(baseSiswa('GetDaftarSiswa'),{
            headers:{
                "Authorization" : `Bearer ${token}`
            }
        }).then(res => {
            this.siswa = res.data
        })
    }
}
</script>

<style>

</style>