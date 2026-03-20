import { defineStore } from 'pinia'
import { state } from './state'
import * as getters from './getters'
import * as actions from './actions'

export const useInventoryStore = defineStore('inventory', {
  state,
  getters,
  actions,
})
