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
                    <input type="text" class="form-control" v-model="searchKey" placeholder="Cari Jurusan" aria-label="Cari Jurusan" aria-describedby="cariKelas">
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary" @click="cariKelas" type="button">Cari</button>
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
                        <th>Nama Jurusan</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in jurusan" key="item.jurusanID">
                        <td>{{ item.namaJurusan }}</td>
                        <td>
                            <ButtonTable @click="daftarJurusanToggle(item.jurusanID, item.namaJurusan)" class="btn btn-info btn-sm mx-1" text="Daftar Kelas"/>
                            <ButtonTable @click="editToggle(item.jurusanID)" class="btn btn-warning btn-sm mx-1" text="Edit"/>
                            <ButtonTable @click="hapusJurusan(item.jurusanID)" class="btn btn-danger btn-sm mx-1" text="Hapus"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div v-if="!datashow && !daftarJurusanList">
        <div class="row">
            <div class="col-12 my-3">
                <div class="form-group">
                    <label for="namaJurusan">Nama Jurusan</label>
                    <input type="text" class="form-control" v-model="namaJurusan" autocomplete="off">
                    <p  v-if="Error['namaJurusan']" class="text-danger">* {{ Error['namaJurusan'] }}</p>
                </div>
            </div>
            <div class="col-6">
                <ButtonTable @click="tambahToggle" text="Batal" class="btn btn-block btn-danger" />
            </div>
            <div class="col-6">
                <ButtonTable @click="simpanJurusan" text="Simpan" class="btn btn-block btn-success" />
            </div>
        </div>
    </div>
    <div v-if="daftarJurusanList">
        <div class="row">
            <div class="col-12 my-3">
                <h3>{{ namaJurusanTerpilih }}</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-3 offset-9">
                <button type="button" class="btn btn-primary btn-block btn-sm mx-1" @click="tambahJurusanToggle()" data-toggle="modal" data-target="#tambahSiswa">
                    Tambah
                </button>
            </div>
        </div>
        <div class="modal fade" id="tambahSiswa" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <!-- <div class="modal-dialog modal-dialog-centered modal-lg"> -->
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Daftar Kelas</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <table class="table table-sm">
                                    <thead>
                                        <tr>
                                            <th>Nama Kelas</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="item in kelasTanpaJurusan" key="item.KelasID">
                                            <td>{{ item.NamaKelas }}</td>
                                            <td>
                                                <ButtonTable @click="tambahKeJurusan(item.KelasID)" class="btn btn-primary btn-sm mx-1" text="Tambah"/>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-lg-12">
                <table class="table table-sm">
                    <thead>
                        <tr>
                            <th>Nama Kelas</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in kelas" key="item.KelasID">
                            <td>{{ item.NamaKelas }}</td>
                            <td>
                                <ButtonTable @click="hapusKelas(item.KelasID)" class="btn btn-danger btn-sm mx-1" text="Hapus"/>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-6">
                <ButtonTable @click="daftarJurusanToggle()" class="btn btn-danger btn-sm mx-1" text="Kembali" />
            </div>
        </div>
    </div>
</template>

<script>
import querystring from 'querystring';
import axios from 'axios';
import baseJurusan from './apiVariable';
import ButtonTable from './../components/ButtonTable.vue'
import baseSiswa from '../daftarSiswa/apiVariable';

export default {
    name:'App',
    data(){
        return {
            jurusan:[],
            kelas:[],
            token:token,
            title:"Daftar Jurusan",
            datashow:true,
            namaJurusan:"",
            namaJurusanTerpilih:"",
            idJurusanSelect:0,
            kelasTanpaJurusan:[],
            jurusanID:0,
            searchKey:"",
            Error:{},
            daftarJurusanList:false
        }
    },
    components:{
        ButtonTable
    },
    methods:{
        tambahToggle(){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Jurusan" : "Tambah Jurusan"
            this.namaJurusan = ""
            this.jurusanID = 0
            this.Error = {}
        },
        editToggle(idJurusanSelect){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Jurusan" : "Edit Jurusan"
            this.Error = {}
            axios.get(baseJurusan(`GetJurusan/${idJurusanSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${this.token}`
                }
            }).then(res => {
                this.namaJurusan = res.data.NamaJurusan
                this.jurusanID = res.data.JurusanID
            })
        },
        hapusJurusan(idJurusanSelect){
            axios.delete(baseJurusan(`Delete/${idJurusanSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${this.token}`
                }
            })
            this.jurusan.forEach((item, index) => {
                if (item.jurusanID == idJurusanSelect) {
                    this.jurusan.splice(index, 1);
                    return
                }
            })
        },
        daftarJurusanToggle(idJurusanSelect, namaJurusan = ""){
            this.daftarJurusanList = !this.daftarJurusanList
            this.datashow = !this.datashow
            this.idJurusanSelect = idJurusanSelect
            this.title = this.title == "Daftar Kelas Jurusan" ? "Daftar Jurusan" : "Daftar Kelas Jurusan"
            this.namaJurusanTerpilih = namaJurusan

            if (this.daftarJurusanList == true && this.datashow == false) {
                axios.get(baseJurusan(`GetListKelas/${idJurusanSelect}`), {
                    headers:{
                        "Authorization" : `Bearer ${this.token}`
                    }
                }).then(res => {
                    this.kelas = res.data
                })
            }
        },
        simpanJurusan(){
            let data = {
                jurusanID : this.jurusanID,
                namaJurusan: this.namaJurusan,
            }
            this.title = "Daftar Jurusan"
            if (data.jurusanID > 0) {
                axios.put(baseJurusan(`Edit?area=Jurusan`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${this.token}`
                    }
                }).then(res => {
                    this.changeJurusan(res);
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }else{
                axios.post(baseJurusan(`Create?area=Jurusan`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${this.token}`
                    }
                }).then(res => {
                    this.addJurusan(res)
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }
        },
        cariKelas(){
            let data = {
                namaJurusan: this.searchKey
            }
            if (data.namaJurusan.length > 0) {
                axios.post(baseJurusan(`Search?area=Kelas`), querystring.stringify(data, {
                    headers:{
                        "Authorization" : `Bearer ${this.token}`
                    }
                })).then((res) => {
                    this.kelas = res.data
                }).catch((res) => {
                    if (res.response.data) {
                        console.log(res.response)
                    }
                })
            }else{
                axios.get(baseJurusan('GetDaftarKelas'), {
                    headers:{
                        "Authorization" : `Bearer ${this.token}`
                    }
                }).then(res => {
                    this.kelas = res.data
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
        changeJurusan(res){
            this.jurusan.forEach((item, index) => {
                if (item.jurusanID == res.data.jurusanID) {
                    this.jurusan[index].namaJurusan = res.data.namaJurusan
                    return
                }
            })
        },
        addJurusan(res){
            this.jurusan.push({
                namaJurusan:res.data.NamaJurusan,
                jurusanID:res.data.JurusanID
            })
        },
        hapusKelas(idKelasSelect){
            axios.delete(baseJurusan(`HapusKelas/${idKelasSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${this.token}`
                }
            }).then(res => {
                this.kelas.forEach((item, index) => {
                    if (item.KelasID == idKelasSelect) {
                        this.kelas.splice(index, 1)
                        return
                    }
                })
            })
        },
        tambahJurusanToggle(){
            axios.get(baseJurusan('GetKelasTanpaJurusan'), {
                headers:{
                    "Authorization" : `Bearer ${this.token}`
                }
            }).then(res => {
                this.kelasTanpaJurusan = res.data
            })
        },
        tambahKeJurusan(idKelasSelect) {
            let put = {
                KelasID: idKelasSelect,
                JurusanID: this.idJurusanSelect
            }
            axios.put(baseJurusan(`SetJurusan`), querystring.stringify(put), {
                headers:{
                    "Authorization" : `Bearer ${this.token}`
                }
            }).then(res => {
                this.kelasTanpaJurusan.forEach((item, index) => {
                    if (item.KelasID == idKelasSelect) {
                        this.kelas.push(item)
                        this.kelasTanpaJurusan.splice(index, 1)
                        return
                    }
                })
            })
        }
    },
    created(){
        axios.get(baseJurusan('GetDaftarJurusan'), {
            headers:{
                "Authorization" : `Bearer ${this.token}`
            }
        }).then(res => {
            this.jurusan = res.data
        })
    }
}
</script>

<style>

</style>