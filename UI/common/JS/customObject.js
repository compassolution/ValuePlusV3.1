/*
******************************************
           自定义javascript类对象                              
******************************************
*/



/*
******************************************
           自定义哈希表类    
           **************
    var myhash=new Hashtable();                //创建哈希表 
    myhash.add("name","张三");                 //添加键和值
    if(myhash.contains("name"))                //判断是否存在name键 
    alert(myhash.items("name"));               //根据指定name键显示哈希表的值                           
******************************************
*/
function Hashtable() 
{ 
    this._hash = new Object();  // 创建Object对象 

    //哈希表的添加方法 
    this.add = function(key,value){ 
                if(typeof(key)!="undefined"){ 
                    if(this.contains(key)==false){ 
                          this._hash[key]=typeof(value)=="undefined"?null:value; 
                          return true; 
                    } else { 
                           return false; 
                    } 
                } else { 
                          return false; 
                } 
            } 

    //哈希表的移除方法 
    this.remove = function(key){delete this._hash[key];} 
    
    //哈希表内部键的数量 
    this.count = function(){var i=0;for(var k in this._hash){i++;} return i;} 

    //通过键值获取哈希表的值 
    this.items = function(key){return this._hash[key];} 

    //在哈希表中判断某个值是否存在
    this.contains  = function(key){ return typeof(this._hash[key])!= "undefined";} 

    //清空哈希表内容的方法 
    this.clear = function(){for(var k in this._hash){delete this._hash[k];}} 
} 
