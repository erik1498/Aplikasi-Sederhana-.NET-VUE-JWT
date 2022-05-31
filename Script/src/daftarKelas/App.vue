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
                    <input type="text" class="form-control" v-model="searchKey" placeholder="Cari Kelas" aria-label="Cari Kelas" aria-describedby="cariKelas">
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
                        <th>Nama Kelas</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in kelas" key="item.KelasID">
                        <td>{{ item.NamaKelas }}</td>
                        <td>
                            <ButtonTable @click="daftarSiswaToggle(item.KelasID, item.NamaKelas)" class="btn btn-info btn-sm mx-1" text="Daftar Siswa"/>
                            <ButtonTable @click="editToggle(item.KelasID)" class="btn btn-warning btn-sm mx-1" text="Edit"/>
                            <ButtonTable @click="hapusKelas(item.KelasID)" class="btn btn-danger btn-sm mx-1" text="Hapus"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div v-if="!datashow && !daftarSiswaList">
        <div class="row">
            <div class="col-12 my-3">
                <div class="form-group">
                    <label for="NamaKelas">Nama Kelas</label>
                    <input type="text" class="form-control" v-model="NamaKelas" autocomplete="off">
                    <p  v-if="Error['NamaKelas']" class="text-danger">* {{ Error['NamaKelas'] }}</p>
                </div>
            </div>
            <div class="col-6">
                <ButtonTable @click="tambahToggle" text="Batal" class="btn btn-block btn-danger" />
            </div>
            <div class="col-6">
                <ButtonTable @click="simpanKelas" text="Simpan" class="btn btn-block btn-success" />
            </div>
        </div>
    </div>
    <div v-if="daftarSiswaList">
        <div class="row">
            <div class="col-12 my-3">
                <h3>{{ NamaKelasTerpilih }}</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-3 offset-9">
                <button type="button" class="btn btn-primary btn-block btn-sm mx-1" @click="tambahSiswaToggle()" data-toggle="modal" data-target="#tambahSiswa">
                    Tambah
                </button>
            </div>
        </div>
        <div class="modal fade" id="tambahSiswa" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <!-- <div class="modal-dialog modal-dialog-centered modal-lg"> -->
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Daftar Siswa</h5>
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
                                            <th>Nama Siswa</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="item in siswaTanpaKelas" key="item.SiswaID">
                                            <td>{{ item.NamaSiswa }}</td>
                                            <td>
                                                <ButtonTable @click="tambahKekelas(item.SiswaID)" class="btn btn-primary btn-sm mx-1" text="Tambah"/>
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
                            <th>Nama Siswa</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in siswa" key="item.SiswaID">
                            <td>{{ item.NamaSiswa }}</td>
                            <td>
                                <ButtonTable @click="hapusSiswa(item.SiswaID)" class="btn btn-danger btn-sm mx-1" text="Hapus"/>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-6">
                <ButtonTable @click="daftarSiswaToggle()" class="btn btn-danger btn-sm mx-1" text="Kembali" />
            </div>
        </div>
    </div>
</template>

<script>
import querystring from 'querystring';
import axios from 'axios';
import baseKelas from './apiVariable';
import ButtonTable from './../components/ButtonTable.vue'
import baseSiswa from '../daftarSiswa/apiVariable';

export default {
    name:'App',
    data(){
        return {
            kelas:[],
            siswa:[],
            token:token,
            title:"Daftar Kelas",
            datashow:true,
            NamaKelas:"",
            NamaKelasTerpilih:"",
            idKelasSelect:0,
            siswaTanpaKelas:[],
            KelasID:0,
            searchKey:"",
            Error:{},
            daftarSiswaList:false
        }
    },
    components:{
        ButtonTable
    },
    methods:{
        tambahToggle(){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Kelas" : "Tambah Kelas"
            this.NamaKelas = ""
            this.kelasID = 0
            this.Error = {}
        },
        editToggle(idKelasSelect){
            this.datashow = !this.datashow
            this.title = this.datashow ? "Daftar Kelas" : "Edit Kelas"
            this.Error = {}
            axios.get(baseKelas(`GetKelas/${idKelasSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            }).then(res => {
                this.NamaKelas = res.data.NamaKelas
                this.kelasID = res.data.KelasID
            })
        },
        hapusKelas(idKelasSelect){
            axios.delete(baseKelas(`Delete/${idKelasSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            })
            this.kelas.forEach((item, index) => {
                if (item.KelasID == idKelasSelect) {
                    this.kelas.splice(index, 1);
                    return
                }
            })
        },
        daftarSiswaToggle(idKelasSelect, NamaKelas = ""){
            this.daftarSiswaList = !this.daftarSiswaList
            this.datashow = !this.datashow
            this.idKelasSelect = idKelasSelect
            this.title = this.title == "Daftar Siswa Kelas" ? "Daftar Kelas" : "Daftar Siswa Kelas"
            this.NamaKelasTerpilih = NamaKelas

            if (this.daftarSiswaList == true && this.datashow == false) {
                axios.get(baseKelas(`GetListSiswa/${idKelasSelect}`), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.siswa = res.data
                })
            }
        },
        simpanKelas(){
            let data = {
                KelasID : this.kelasID,
                NamaKelas: this.NamaKelas
            }
            this.title = "Daftar Kelas"
            if (data.KelasID > 0) {
                axios.put(baseKelas(`Edit?area=Kelas`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                    this.changeKelas(res);
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }else{
                axios.post(baseKelas(`Create?area=Kelas`), querystring.stringify(data), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                }).then(res => {
                   this.addKelas(res)
                    this.datashow = !this.datashow
                }).catch((error) => {
                    this.createError(error);
                })
            }
        },
        cariKelas(){
            let data = {
                NamaKelas: this.searchKey
            }
            if (data.NamaKelas.length > 0) {
                axios.post(baseKelas(`Search?area=Kelas`), querystring.stringify(data, {
                    headers:{
                        "Authorization" : `Bearer ${token}`
                    }
                })).then((res) => {
                    this.kelas = res.data
                }).catch((res) => {
                    if (res.response.data) {
                        console.log(res.response)
                    }
                })
            }else{
                axios.get(baseKelas('GetDaftarKelas'), {
                    headers:{
                        "Authorization" : `Bearer ${token}`
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
        changeKelas(res){
            this.kelas.forEach((item, index) => {
                if (item.KelasID == res.data.kelasID) {
                    this.kelas[index].NamaKelas = res.data.namaKelas
                    return
                }
            })
        },
        addKelas(res){
            this.kelas.push({
                NamaKelas:res.data.NamaKelas,
                KelasID:res.data.KelasID
            })
        },
        hapusSiswa(idSiswaSelect){
            axios.delete(baseKelas(`HapusSiswa/${idSiswaSelect}`), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            }).then(res => {
                this.siswa.forEach((item, index) => {
                    if (item.SiswaID == idSiswaSelect) {
                        this.siswa.splice(index, 1)
                        return
                    }
                })
            })
        },
        tambahSiswaToggle(){
            axios.get(baseSiswa('GetSiswaTanpaKelas'), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            }).then(res => {
                this.siswaTanpaKelas = res.data
            })
        },
        tambahKekelas(idSiswaSelect) {
            let put = {
                SiswaID: idSiswaSelect,
                KelasID: this.idKelasSelect
            }
            axios.put(baseSiswa(`SetKelas`), querystring.stringify(put), {
                headers:{
                    "Authorization" : `Bearer ${token}`
                }
            }).then(res => {
                this.siswaTanpaKelas.forEach((item, index) => {
                    if (item.SiswaID == idSiswaSelect) {
                        this.siswa.push(item)
                        this.siswaTanpaKelas.splice(index, 1)
                        return
                    }
                })
            })
        }
    },
    created(){
        axios.get(baseKelas('GetDaftarKelas'), {
            headers:{
                "Authorization" : `Bearer ${token}`
            }
        }).then(res => {
            this.kelas = res.data
        })
    }
}
</script>

<style>

</style>