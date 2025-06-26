import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { familyMembersAPI, familyTreeAPI } from '../services/api'

export const useFamilyTreeStore = defineStore('familyTree', () => {
  const members = ref([])
  const currentTree = ref(null)
  const loading = ref(false)
  const statistics = ref(null)
  
  const membersByGender = computed(() => {
    return {
      male: members.value.filter(m => m.gender?.toLowerCase() === 'male'),
      female: members.value.filter(m => m.gender?.toLowerCase() === 'female'),
      other: members.value.filter(m => !m.gender || !['male', 'female'].includes(m.gender?.toLowerCase()))
    }
  })
  
  const livingMembers = computed(() => 
    members.value.filter(m => m.isAlive)
  )
  
  const deceasedMembers = computed(() => 
    members.value.filter(m => !m.isAlive)
  )
  
  // Fetch all family members
  const fetchMembers = async () => {
    loading.value = true
    try {
      const response = await familyMembersAPI.getAll()
      members.value = response.data
      return { success: true }
    } catch (error) {
      console.error('Error fetching family members:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to fetch family members' 
      }
    } finally {
      loading.value = false
    }
  }
  
  // Create new family member
  const createMember = async (memberData) => {
    loading.value = true
    try {
      const response = await familyMembersAPI.create(memberData)
      members.value.push(response.data)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Error creating family member:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to create family member' 
      }
    } finally {
      loading.value = false
    }
  }
  
  // Update family member
  const updateMember = async (id, memberData) => {
    loading.value = true
    try {
      const response = await familyMembersAPI.update(id, memberData)
      const index = members.value.findIndex(m => m.id === id)
      if (index !== -1) {
        members.value[index] = response.data
      }
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Error updating family member:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to update family member' 
      }
    } finally {
      loading.value = false
    }
  }
  
  // Delete family member
  const deleteMember = async (id) => {
    loading.value = true
    try {
      await familyMembersAPI.delete(id)
      members.value = members.value.filter(m => m.id !== id)
      return { success: true }
    } catch (error) {
      console.error('Error deleting family member:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to delete family member' 
      }
    } finally {
      loading.value = false
    }
  }
  
  // Search family members
  const searchMembers = async (searchTerm) => {
    if (!searchTerm.trim()) {
      return members.value
    }
    
    try {
      const response = await familyMembersAPI.search(searchTerm)
      return response.data
    } catch (error) {
      console.error('Error searching family members:', error)
      return []
    }
  }
  
  // Fetch family tree starting from root
  const fetchFamilyTree = async (rootId) => {
    loading.value = true
    try {
      const response = await familyTreeAPI.getTree(rootId)
      currentTree.value = response.data
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Error fetching family tree:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to fetch family tree' 
      }
    } finally {
      loading.value = false
    }
  }
  
  // Get potential tree roots
  const fetchTreeRoots = async () => {
    try {
      const response = await familyTreeAPI.getRoots()
      return response.data
    } catch (error) {
      console.error('Error fetching tree roots:', error)
      return []
    }
  }
  
  // Fetch family statistics
  const fetchStatistics = async () => {
    try {
      const response = await familyTreeAPI.getStatistics()
      statistics.value = response.data
      return response.data
    } catch (error) {
      console.error('Error fetching statistics:', error)
      return null
    }
  }
  
  // Add spouse relationship
  const addSpouse = async (spouseData) => {
    try {
      const response = await familyMembersAPI.addSpouse(spouseData)
      // Refresh the affected members
      await fetchMembers()
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Error adding spouse:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to add spouse relationship' 
      }
    }
  }
  
  // Remove spouse relationship
  const removeSpouse = async (spouseId) => {
    try {
      await familyMembersAPI.removeSpouse(spouseId)
      // Refresh the affected members
      await fetchMembers()
      return { success: true }
    } catch (error) {
      console.error('Error removing spouse:', error)
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to remove spouse relationship' 
      }
    }
  }
  
  // Get member by ID
  const getMemberById = (id) => {
    return members.value.find(m => m.id === id)
  }
  
  // Get children of a member
  const getChildren = (memberId) => {
    return members.value.filter(m => m.fatherId === memberId || m.motherId === memberId)
  }
  
  // Get parents of a member
  const getParents = (member) => {
    const parents = []
    if (member.fatherId) {
      const father = getMemberById(member.fatherId)
      if (father) parents.push(father)
    }
    if (member.motherId) {
      const mother = getMemberById(member.motherId)
      if (mother) parents.push(mother)
    }
    return parents
  }
  
  return {
    members,
    currentTree,
    loading,
    statistics,
    membersByGender,
    livingMembers,
    deceasedMembers,
    fetchMembers,
    createMember,
    updateMember,
    deleteMember,
    searchMembers,
    fetchFamilyTree,
    fetchTreeRoots,
    fetchStatistics,
    addSpouse,
    removeSpouse,
    getMemberById,
    getChildren,
    getParents
  }
}) 