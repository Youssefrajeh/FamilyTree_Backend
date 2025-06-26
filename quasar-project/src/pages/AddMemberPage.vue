<template>
  <q-page class="flex flex-center add-member-page">
    <q-card class="q-pa-xl shadow-4">
      <div class="text-h5 text-center text-weight-bold q-mb-lg">Add Family Member</div>
      <q-form @submit="handleAdd" class="q-gutter-md">
        <q-input v-model="form.name" label="Full Name" outlined :rules="[val => !!val || 'Name is required']" />
        <q-input v-model="form.relationship" label="Relationship" outlined :rules="[val => !!val || 'Relationship is required']" />
        <q-input v-model="form.birthdate" label="Birthdate" type="date" outlined />
        <q-input v-model="form.notes" label="Notes" type="textarea" outlined />
        <q-btn type="submit" color="primary" label="Add Member" class="full-width q-mt-md" :loading="loading" />
      </q-form>
      <q-banner v-if="success" class="bg-positive text-white q-mt-md">
        <q-icon name="check_circle" /> Family member added!
      </q-banner>
    </q-card>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
const form = ref({ name: '', relationship: '', birthdate: '', notes: '' })
const loading = ref(false)
const success = ref(false)
const handleAdd = () => {
  loading.value = true
  setTimeout(() => {
    loading.value = false
    success.value = true
    form.value = { name: '', relationship: '', birthdate: '', notes: '' }
  }, 1000)
}
</script>

<style scoped>
.add-member-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #e3ecfc 0%, #b6c8f9 100%);
}
</style> 