<template>
  <q-page class="family-tree-home">
    <div class="header-section">
      <div class="main-title">
        <q-icon name="groups" size="48px" color="primary" />
        <span>Family Tree Manager</span>
      </div>
      <div class="subtitle">
        Create, manage, and visualize your family history
      </div>
    </div>
    <div class="container main-content">
      <q-card class="welcome-card q-pa-xl shadow-4">
        <div class="text-h3 text-primary text-weight-bold q-mb-md">Welcome to Family Tree Manager</div>
        <div class="text-body1 q-mb-lg">Start building your family tree today. Create profiles, add relationships, and visualize your family history.</div>
        <div class="row justify-center q-gutter-md">
          <q-btn color="primary" rounded size="lg" icon="login" label="Get Started" class="q-px-xl" @click="showLogin = true" />
          <q-btn color="secondary" rounded size="lg" icon="info" label="Learn More" class="q-px-xl" flat />
        </div>
      </q-card>
      <div class="features-section q-mt-xl">
        <div class="text-h5 text-center text-weight-bold q-mb-lg">Features</div>
        <div class="features-grid row q-col-gutter-xl q-row-gutter-lg justify-center">
          <q-card class="feature-card col-xs-12 col-sm-6 col-md-3 q-pa-lg shadow-2 flex flex-center">
            <q-icon name="person" size="36px" color="primary" />
            <div class="text-h6 q-mt-md">Family Members</div>
            <div class="text-body2 q-mt-sm">Add and manage family member profiles with photos and details.</div>
          </q-card>
          <q-card class="feature-card col-xs-12 col-sm-6 col-md-3 q-pa-lg shadow-2 flex flex-center">
            <q-icon name="account_tree" size="36px" color="primary" />
            <div class="text-h6 q-mt-md">Tree Visualization</div>
            <div class="text-body2 q-mt-sm">Interactive family tree with zoom, pan, and expand features.</div>
          </q-card>
          <q-card class="feature-card col-xs-12 col-sm-6 col-md-3 q-pa-lg shadow-2 flex flex-center">
            <q-icon name="search" size="36px" color="primary" />
            <div class="text-h6 q-mt-md">Search & Filter</div>
            <div class="text-body2 q-mt-sm">Find family members quickly with advanced search options.</div>
          </q-card>
          <q-card class="feature-card col-xs-12 col-sm-6 col-md-3 q-pa-lg shadow-2 flex flex-center">
            <q-icon name="security" size="36px" color="primary" />
            <div class="text-h6 q-mt-md">Secure & Private</div>
            <div class="text-body2 q-mt-sm">Your family data is protected with secure authentication.</div>
          </q-card>
        </div>
      </div>
    </div>
    <q-dialog v-model="showLogin">
      <login-page @close="showLogin = false" />
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import LoginPage from './LoginPage.vue'

const showLogin = ref(false)
</script>

<style scoped>
.family-tree-home {
  min-height: 100vh;
  background: linear-gradient(135deg, #e3ecfc 0%, #b6c8f9 100%);
  padding-bottom: 4rem;
}

.header-section {
  padding: 4rem 0 2rem;
  text-align: center;
  color: #222;
}

.main-title {
  font-size: 2.5rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  color: #1976d2;
}

.subtitle {
  font-size: 1.2rem;
  opacity: 0.8;
  color: #333;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.main-content {
  padding: 2rem 0;
}

.welcome-card {
  max-width: 600px;
  margin: 0 auto 3rem;
  text-align: center;
  border-radius: 18px;
  background: #fff;
}

.features-section {
  margin-top: 4rem;
}

.features-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 2rem;
}

.feature-card {
  min-width: 220px;
  max-width: 300px;
  border-radius: 16px;
  background: #fff;
  transition: transform 0.2s, box-shadow 0.2s;
  align-items: center;
  text-align: center;
}
.feature-card:hover {
  transform: translateY(-6px) scale(1.03);
  box-shadow: 0 8px 32px rgba(25, 118, 210, 0.12);
}

@media (max-width: 900px) {
  .features-grid {
    flex-direction: column;
    align-items: center;
  }
}
</style>
