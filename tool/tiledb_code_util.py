 # -*- coding: utf-8 -*-
import os,sys
import re
import inspect
import codecs


import cpp_header_parser as chp 

import glob

import json
import fnmatch

#import CppToPyUtil #

import copy

######
def get_chp(filename,declspec_list=[]):
    result=None
    try:
        result=chp.CppHeader(filename,declspec_list=declspec_list)
    except chp.CppParseError as e:
        print(e)
        result=None
    return result
######
########################
def has_keywords_for_ptr(name,keywords_for_ptr,ignore_keywords_for_ptr):
    return ( is_any_of_words_in_str(keywords_for_ptr, name) ) or (not is_any_of_words_in_str(ignore_keywords_for_ptr, name))
def is_any_of_words_in_str(words=[],s=''):
    for w in words:
        if w in s:
            return True
    return False
def is_all_of_words_in_str(words=[],s=''):
    for w in words:
        if not w in s:
            return False
    return True

######
def get_files_by_pattern(directory='./',pattern='*'):
    filepattern=("%s/%s") % (directory,pattern)
    return glob.glob(filepattern)
def get_filenames_by_pattern(directory='./',pattern='*'):
    files=get_files_by_pattern(directory,pattern)
    if files is None or len(files)==0:
        return None
    filenames=[]
    for f in files:
        filename=f.split('/')[-1]
        filename=filename.split('\\')[-1]
        filenames.append(filename)
    return filenames
def get_subdirs_by_pattern(directory='./',pattern='*'):
    dirpattern=("%s/%s") % (directory,pattern)
    subdirs=glob.glob(dirpattern)
    results=[]
    for d in subdirs:
        if os.path.isdir(d):
            results.append(d)
    return results
def get_sub_name_for_path_or_file(fullname):
    name=fullname.replace('\\','/')
    items=name.split('/')
    return items[-1]
def get_upper_dir_for_path_or_file(fullname):
    name=fullname.replace('\\','/')
    items=name.split('/')
    return '/'.join(items[0:-1])
def get_files_by_pattern_r(directory='./',pattern='*'):
    results=[]
    for dirpath,dirnames,files in os.walk(directory):
        for f in fnmatch.filter(files,pattern):
            results.append(os.path.join(dirpath,f))
    return results
def get_subdirs_r(directory='./',pattern='*'):
    results=[]
    for dirpath,dirnames,files in os.walk(directory):
        for d in fnmatch.filter(dirnames,pattern):
            results.append(os.path.join(dirpath,d))
    return results
######################
def convert_tiledb_enum_file(inputfile='../../TileDB/tiledb/sm/c_api/tiledb_enum.h',outputfile='./tiledb_enum.h'):
    outfile=open(outputfile,'w')
    with codecs.open(inputfile,'r') as f:
        lines=f.readlines()
        isenum_block=False
        for line in lines:
            line=line.replace('\n','')
            if "#ifdef" in line and 'TILEDB_' in line and '_ENUM' in line:
                isenum_block=True
                items=line.split()
                typename=items[1].replace('ENUM','t').lower()
                if typename=='tiledb_object_type_t':
                    typename='tiledb_object_t'
                outfile.write('enum %s{ //%s\n' % (typename,line))
            elif '#endif' in line:
                outfile.write('};//%s\n' % (line))
                isenum_block=False
            elif isenum_block and ('(' in line) and (')' in line) and ('TILEDB_' in line) and ('_ENUM' in line):
                items=line.split('=')
                value_part=items[-1].replace('\n','')
                name_part=items[0].split('(')[-1].split(')')[0]
                outfile.write('    TILEDB_%s = %s //%s\n' % (name_part,value_part,line) )
            else:
                outfile.write('%s\n' % (line))
    outfile.close()
    return

######
def get_default_cfginfo():
    cfginfo={"dir":"../cpp/src/tiledb/cxx_api", "namespaces":["tiledb"],"outputdir":"../cpp/src/tiledb_pybind","ignore_file_keywords":["XTAppBase","XTConfig","XTTimer","XTSingleton","XTStartExit"]}
    ignore_method_keywords=['compareBar','operator','std::thread','boost::array','boost::signals2','boost::unordered','std::unordered','tbb::concurrent_','sf::contfree_safe_ptr']    
    ignore_method_keywords.extend(["emplace","ConcurrentQueue"])
#    ignore_method_keywords.extend(['std::vector<StringMapPtr'])
    ignore_method_keywords.extend(['std::list','std::set','std::deque','std::map<int, int'])
#    ignore_method_keywords.extend(['static T GCD','static T RoundUp','T fromString','const T'])
#    ignore_method_keywords.extend(['google::protobuf::RepeatedField','google::protobuf::Map'])
#    ignore_method_keywords.extend(['default_instance','New(','Swap(','CopyFrom(','MergeFrom(','ivec('])
    ignore_method_keywords.extend(['time_t'])
##ignore_method_keywords.extend(['Ptr','_ptr'])
#    ignore_method_keywords.extend(['getApi()','md()','trd()'])
#    ignore_method_keywords.extend(['toRaw','fromRaw','strOfRaw','csvStrOfRaw','processOn','RawPtr'])
    ignore_method_keywords.extend(['FILE','ptr()','std::function','std::vector<tiledb::Dimension>','attributes()','std::map<std::string,tiledb::Attribute>','arrow::Array','arrow::Table','arrow::Schema']) # for tiledb
    ignore_method_keywords.extend(['tiledb_array_t ','tiledb_buffer_t','tiledb_buffer_list_t','tiledb_config_t','tiledb_config_iter_t'])#for tiledb
    ignore_method_keywords.extend(['tiledb_ctx_t','tiledb_error_t','tiledb_attribute_t','tiledb_array_schema_t','tiledb_dimension_t','tiledb_query_condition_t']) #for tiledb
    ignore_method_keywords.extend(['tiledb_domain_t','tiledb_filter_t ','tiledb_filter_list_t','submit_async','tiledb_query_status_t'])#,'tiledb_query_t']) #for tiledb
    ignore_method_keywords.extend(['tiledb_vfs_t','tiledb_vfs_fh_t','const void','const void*','void *','std::pair<T,','ConfigIter','cell_num'])#for tiledb
    cfginfo['ignore_method_keywords']=ignore_method_keywords
#
    ignore_ptr_keywords=['Enum','Util','Comparator','Mgr','CThostFtdc']
    cfginfo['ignore_ptr_keywords']=ignore_ptr_keywords
    ptr_keywords=['MgrData','CfgData']
    cfginfo['ptr_keywords']=ptr_keywords 
    ignore_class_keywords=[]
    cfginfo['ignore_class_keywords']=ignore_class_keywords
    cfginfo['outputdir']="./temp"
    cfginfo['include_str']='#include "common_include.h" \n\n'
    return cfginfo
def get_file_cfginfos_for_dir_cfginfo(dir_cfginfo):
    result=[]
    dirname=dir_cfginfo.get("dir","./")
    default_cfginfo=get_default_cfginfo()
    ns=dir_cfginfo.get("namespaces",['tiledb'])
    outputdir=dir_cfginfo.get('outputdir',"./temp")
    hfilepattern=dir_cfginfo.get('hfilepattern','*.h') 
#
    hfiles=get_files_by_pattern(dirname,hfilepattern)
    if hfiles is None or len(hfiles)==0:
        return ""
    hfiles=[f.replace("\\","/") for f in hfiles]  
    for hfile in hfiles:
        if is_any_of_words_in_str(dir_cfginfo['ignore_file_keywords'],hfile):
            linfo='ignore file:%s' % (hfile)
            print(linfo)
            continue
        else:
            linfo='start to process %s' % (hfile)
            print(linfo)   
        file_cfginfo=copy.deepcopy(default_cfginfo)
        file_cfginfo.update(dir_cfginfo)
        file_cfginfo['filename']=hfile
        result.append(file_cfginfo)
    return result
#########
def get_pybind_for_file_cfginfo(file_cfginfo):
    dd={}
##    base_cfginfo=get_base_cfginfo()
    if file_cfginfo is None:
        return dd
    if not "filename" in file_cfginfo:
        return dd
#    
    ignore_ptr_keywords=file_cfginfo.get('ignore_ptr_keywords',[])
    ptr_keywords=file_cfginfo.get('ptr_keywords',[])
    ignore_class_keywords=file_cfginfo.get('ignore_class_keywords',[])
    ignore_method_keywords=file_cfginfo.get('ignore_method_keywords',[])
#  
    outputdir=file_cfginfo.get('outputdir','./temp')
    filename=file_cfginfo['filename']
    tempitems=filename.split('/')
    full_filename=tempitems[-1]
    tempitems=tempitems[-1].split('.')
    tempitem=tempitems[0]
    stem_filename=tempitem
    protobuf_inttypes=["google::protobuf::int32","google::protobuf::uint32","google::protobuf::int64","google::protobuf::uint64"]
    ns=['tiledb']
    if 'namespaces' in file_cfginfo:
        ns=file_cfginfo['namespaces']
    ns0=''
    if len(ns)>0:
        ns0=ns[0]
    include_str_hfile='#pragma once\n' 
    DEF_STR='%s_PYBIND_%s_H' % (ns0,stem_filename)
    DEF_STR=DEF_STR.upper()
    include_str_hfile='%s#ifndef %s\n' % (include_str_hfile,DEF_STR)
    include_str_hfile='%s#define %s\n\n' % (include_str_hfile,DEF_STR)
    include_str_hfile='%s%s' % (include_str_hfile,file_cfginfo.get("include_str",""))
    include_str = '#include "%s"' % (full_filename)
    include_str_hfile='%s\n%s\n' % (include_str_hfile,include_str)        
    cppheader=get_chp(filename)  #parse head file
    if cppheader is None:
        linfo='can not parse hfile:%s' % (filename)
        print(linfo)
        return dd
    if 'pb.h' in cppheader.headerFileName and 'Data' in cppheader.headerFileName:
        tempitems=filename.split('/')
        typedef_file='typedef_' + tempitems[-1]
#        include_typedef_file_str='/'.join(tempitems)
        include_str='%s\n#include "%s"\n' % (include_str,typedef_file)
        include_str_hfile='%s\n#include "%s"\n' % (include_str_hfile, typedef_file)
    enumtype_dict={}    
    for enumt in cppheader.enums:
        if enumt['name'].endswith('_enumtype'):
            tempname=enumt['name'][:-9]
            enumtype_dict[tempname]=enumt    
    class_str_dict={}
    class_str=''
    enum_str=''
    class_str_hfile=''
    class_str_cppfile=''
    enum_str_hfile=''
    enum_str_cppfile=''
    init_module_str=''
    for e in cppheader.enums:
        enum_str='%s\tpy::enum_<%s::%s>(m,"%s")\n' % (enum_str,e['namespace'],e['name'],e['name'])
        enum_str_hfile='%svoid init_%s_%s(pybind11::module& m);\n' % (enum_str_hfile,e['namespace'],e['name'])
        enum_str_cppfile='%svoid init_%s_%s(pybind11::module& m) {\n' % (enum_str_cppfile,e['namespace'],e['name'])
        enum_str_cppfile='%s\tpybind11::enum_<%s::%s>(m,"%s")\n' % (enum_str_cppfile,e['namespace'],e['name'],e['name'])
        init_module_str='%s\tinit_%s_%s(m);\n' % (init_module_str,e['namespace'],e['name'])
        for ev in e['values']:
            enum_name=ev['name']
            enum_str='%s\t\t.value("%s", %s::%s)\n' % (enum_str, enum_name, e['namespace'], e['name'])
            enum_str_cppfile='%s\t\t.value("%s", %s::%s)\n' % (enum_str_cppfile, enum_name, e['namespace'], ev['name'])
        enum_str='%s\t\t.export_values();\n\n' % (enum_str)
        enum_str_cppfile='%s\t\t.export_values();\n\n' % (enum_str_cppfile)
        enum_str_cppfile='%s}\n\n' % (enum_str_cppfile)

    for ck in cppheader.classes.keys():
        c=cppheader.classes[ck]
        if not c['namespace'] in ns:
            linfo='ignore class namespace %s::%s'  % (c['namespace'],c['name'])
            print(linfo)
            continue
        if is_any_of_words_in_str(ignore_class_keywords,ck):
            linfo='ignore class keywords %s::%s'  % (c['namespace'],c['name'])
            print(linfo)
            continue
        linfo='start to process class %s::%s'  % (c['namespace'],c['name'])
        if ck in enumtype_dict.keys():
#            enum_str='%s\tpy::class_<%s> %s(m,"%s");\n\n' % (enum_str,ck,ck.lower(),ck)
#            enum_str='%s\tpy::enum_<%s::%s>(%s,"%s")\n' % (enum_str,ck,ck+"_enumtype",ck.lower(),ck)
            enum_str='%s\tpy::enum_<%s::%s>(m,"%s")\n' % (enum_str,c['namespace'],ck+"_enumtype",ck)
            enum_str_hfile='%svoid init_%s_%s(pybind11::module& m);\n' % (enum_str_hfile,c['namespace'],ck)
            enum_str_cppfile='%svoid init_%s_%s(pybind11::module& m) {\n' % (enum_str_cppfile,c['namespace'],ck)
            enum_str_cppfile='%s\tpybind11::enum_<%s::%s>(m,"%s")\n' % (enum_str_cppfile,c['namespace'],ck+"_enumtype",ck)
            init_module_str='%s\tinit_%s_%s(m);\n' % (init_module_str,c['namespace'],ck)
            enumt=enumtype_dict[ck]
            for enumvalue in enumt['values']:
                enum_name=enumvalue['name']
#                enum_str='%s\t\t.value("%s", %s::%s::%s)\n' % (enum_str,enum_name.replace(ck+"_enumtype_",""),ck,ck+"_enumtype",enumvalue['name'])
                enum_str='%s\t\t.value("%s_%s", %s::%s)\n' % (enum_str,ck, enum_name.replace(ck+"_enumtype_",""), c['namespace'], enumvalue['name'])
                enum_str_cppfile='%s\t\t.value("%s_%s", %s::%s)\n' % (enum_str_cppfile,ck, enum_name.replace(ck+"_enumtype_",""), c['namespace'], enumvalue['name'])
            enum_str='%s\t\t.export_values();\n\n' % (enum_str)
            enum_str_cppfile='%s\t\t.export_values();\n\n' % (enum_str_cppfile)
            enum_str_cppfile='%s}\n\n' % (enum_str_cppfile)
            continue #ignore class for enum type
        if is_any_of_words_in_str(ignore_ptr_keywords,ck) or ck.endswith("Util") or ck.endswith("Type"):
            class_str='%s\tpy::class_<%s::%s>(m,"%s")\n' % (class_str,c['namespace'],c['name'],c['name'])
            class_str_hfile='%s\nvoid init_%s_%s(pybind11::module& m);\n' % (class_str_hfile,c['namespace'],ck)
            class_str_cppfile='%s\nvoid init_%s_%s(pybind11::module& m) {\n' % (class_str_cppfile,c['namespace'],ck)
            class_str_cppfile='%s\tpybind11::class_<%s::%s>(m,"%s")\n' % (class_str_cppfile,c['namespace'],c['name'],c['name'])
            init_module_str='%s\tinit_%s_%s(m);\n' % (init_module_str,c['namespace'],ck)
        elif ck.endswith("Mgr") or ck.endswith("Mask") or ck.endswith("Status") or ck.endswith("TypeHelper"):
            class_str='%s\tpy::class_<%s::%s>(m,"%s")\n' % (class_str,c['namespace'],c['name'],c['name'])
            class_str_hfile='%s\nvoid init_%s_%s(pybind11::module& m);\n' % (class_str_hfile,c['namespace'],ck)
            class_str_cppfile='%s\nvoid init_%s_%s(pybind11::module& m) {\n' % (class_str_cppfile,c['namespace'],ck)
            class_str_cppfile='%s\tpybind11::class_<%s::%s>(m,"%s")\n' % (class_str_cppfile,c['namespace'],c['name'],c['name'])
            init_module_str='%s\tinit_%s_%s(m);\n' % (init_module_str,c['namespace'],ck)
        else:
            class_str='%s\tpy::class_<%s::%s, std::shared_ptr<%s::%s> >(m,"%s")\n' % (class_str,c['namespace'],c['name'],c['namespace'],c['name'],c['name'])#class_str='%s\tpy::class_<%s::%s, std::shared_ptr<%s::%s> >(m,"%s")\n' % (class_str,c['namespace'],c['name'],c['namespace'],c['name'],c['name'])
            class_str_hfile='%s\nvoid init_%s_%s(pybind11::module& m);\n' % (class_str_hfile,c['namespace'],ck)
            class_str_cppfile='%s\nvoid init_%s_%s(pybind11::module& m) {\n' % (class_str_cppfile,c['namespace'],ck)
            class_str_cppfile='%s\tpybind11::class_<%s::%s, std::shared_ptr<%s::%s> >(m,"%s")\n' % (class_str_cppfile,c['namespace'],c['name'],c['namespace'],c['name'],c['name'])#class_str_cppfile='%s\tpybind11::class_<%s::%s, std::shared_ptr<%s::%s> >(m,"%s")\n' % (class_str_cppfile,c['namespace'],c['name'],c['namespace'],c['name'],c['name'])
            init_module_str='%s\tinit_%s_%s(m);\n' % (init_module_str,c['namespace'],ck)
#        class_str='%s\t\t.def(py::init<>())\n' % (class_str)
#        class_str_cppfile='%s\t\t.def(pybind11::init<>())\n' % (class_str_cppfile)
        cm_count={}
        for cm in c['methods']['public']:
            cmname=cm['name']
            cm_count[cmname]=cm_count.get(cmname,0)+1
        for cm in c['methods']['public']:
            cmname=cm['name']
            linestr_api=''
            linestr_pb=''
            linestr_call='('
            namespace_prefix='%s::' % (c['namespace'])
            return_type=cm['rtnType']
            if "static" in return_type:
                method_cast='static_cast<%s (*)(' % (return_type.replace("static",""))
            else:
                method_cast='(%s (%s::%s::*)(' % (return_type,c['namespace'],ck)
#            return_type=CommonUtil.add_prefix_for_words_in_str(namespace_data_type_keywords,return_type,namespace_prefix)
            if cm['virtual']:
                linestr_api=linestr_api+"virtual "
                linestr_pb=linestr_pb+"virtual "   
            linestr_api=linestr_api+ return_type #cm['rtnType']
            linestr_api='\t%s %s(' % (linestr_api,cm['name'])
            linestr_pb=linestr_pb + return_type #cm['rtnType']
            linestr_pb='\t%s %s(' % (linestr_pb, cm['name'])   
            nParams=len(cm['parameters'])
            pcount=0
            for p in cm['parameters']:
                if pcount>0:
                    linestr_api="%s," % (linestr_api)
                    linestr_pb="%s," % (linestr_pb)
                    linestr_call='%s,' % (linestr_call)
                    method_cast='%s,' % (method_cast)
                param_type=p['type']
#                param_type=CommonUtil.add_prefix_for_words_in_str(namespace_data_type_keywords,param_type,namespace_prefix)
                linestr_api="%s %s %s" % (linestr_api,param_type,p['name'])
                linestr_call='%s%s' % (linestr_call,p['name'])
                method_cast='%s%s' % (method_cast,p['type'])
                pcount=pcount+1  
            linestr_api='%s)' % (linestr_api)
            linestr_api=linestr_api.replace("const::google::protobuf::Map", "const ::google::protobuf::Map")
            linestr_pb='%s)' % (linestr_pb)
            linestr_call='%s)' % (linestr_call) 
            if cm['const']:
                if 'static' in return_type or ('static' in cm.keys() and cm['static']):
                    method_cast='%s) const >' % (method_cast)
                else:
                    method_cast='%s) const)' % (method_cast)
            else:
                if 'static' in return_type or ('static' in cm.keys() and cm['static']):
                    method_cast='%s)>' % (method_cast)
                else:
                    method_cast='%s))' % (method_cast)
######                        
            nParams=len(cm['parameters'])
            pcount=0
            param_str=''
            orig_param_str=''
            for p in cm['parameters']:
                if pcount==0:
                    orig_param_str=p['type']
                else:
                    orig_param_str='%s,%s' % (orig_param_str,p['type'])
                param_str='%s, py::arg("%s")' % (param_str,p['name'])    
                pcount=pcount+1
            if cm['constructor']:
                linestr_api=linestr_api.replace('void','')
                linestr_pb=linestr_pb.replace('void','')
            elif cm['destructor']:
                linestr_api=linestr_api.replace('void','')
                linestr_pb=linestr_pb.replace('void','')
                if not '~' in linestr_api:
                    linestr_api=linestr_api.replace(c['name'], ('~%s' % (c['name'])) )
                if not '~' in linestr_pb:
                    linestr_pb=linestr_pb.replace(c['name'], ('~%s' % (c['name']) ) )                              
            if cmname.startswith('mutable_') or cmname.startswith('release_') or cmname.startswith('set_allocated_'):
                ignore_line='//ignore %s\n' %(linestr_api)  #cmname.startswith('has_') or cmname.startswith('clear_') or 
                class_str='%s%s' % (class_str,ignore_line) 
                class_str_cppfile='%s%s' % (class_str_cppfile,ignore_line)
            elif cmname.startswith('set_') and len(cm['parameters'])==1 and (cm['parameters'][0]['type']=='const char *' or cm['parameters'][0]['type']=='char const *') :
                ignore_line='//ignore %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)
            elif cmname.startswith('set_') and len(cm['parameters'])==2 and (cm['parameters'][0]['type']=='const char *' or cm['parameters'][0]['type']=='char const *') and cm['parameters'][1]['type']=='size_t':
                ignore_line='//ignore_constchar %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)
            elif is_any_of_words_in_str(ignore_method_keywords, linestr_api): ## ('operator' in linestr_api): # and cm['constructor']:
                ignore_line='//ignore_keywords %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)
            elif c['namespace']=='tiledb' and   'Type' in linestr_api:
                ignore_line='//ignore_Type %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)   
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)                   
            elif cm['constructor']: #and 'const ' in linestr_api and c['name'] in linestr_api: #ignore copy constructor
                if cm.get('deleted',False):
                    ignore_line='//ignore_constructor_deleted %s\n' %(linestr_api)
                    class_str="%s%s" % (class_str,ignore_line)
                    class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)
                elif c['name'].endswith('Util') or c['name'].endswith('Mgr'):
                    ignore_line='//ignore_constructor_staticclass %s\n' %(linestr_api)
                    class_str="%s%s" % (class_str,ignore_line)
                    class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)                    
                else:
                    class_str='%s\t\t.def(py::init<%s>())\n' % (class_str,orig_param_str) #class_str="%s%s" % (class_str,ignore_line)   
                    class_str_cppfile='%s\t\t.def(py::init<%s>())\n' % (class_str_cppfile,orig_param_str)#class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)
            elif cm['destructor']: #and 'const ' in linestr_api and c['name'] in linestr_api: #ignore copy constructor
                ignore_line='//ignore_destructor %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)   
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)                                               
#            elif 'boost::shared_ptr' in cm['rtnType']:
#                ignore_line='//ignore %s\n' %(linestr_api)
#                class_str="%s%s" % (class_str,ignore_line)  
            elif ('typename' in str(cm.get('template',''))) or ('class' in str(cm.get('template',''))):   
                ignore_line='//ignore_templatefunction %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)   
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)                                                              
            elif ("google::protobuf" in linestr_api and (not is_any_of_words_in_str(protobuf_inttypes,linestr_api)) ) or ( "::google::protobuf::Map" in linestr_api):
                ignore_line='//ignore %s\n' %(linestr_api)
                class_str="%s%s" % (class_str,ignore_line)  
                class_str_cppfile="%s%s" % (class_str_cppfile,ignore_line)       
            else:
                if 'static' in cm['rtnType'] or ('static' in cm.keys() and cm['static']):
                    temp_param_str=orig_param_str
                    if len(temp_param_str)>0 and temp_param_str[0]==',':
                        temp_param_str=temp_param_str.replace(',','',1)
                    if cm_count.get(cmname,0) > 1: # cmname.startswith("set_") and nParams==1 and "string" in method_cast:
                        class_str='%s\t\t.def_static("%s", static_cast< %s (*)(%s) >(&%s::%s::%s))\n' % (class_str,cmname,cm['rtnType'],temp_param_str, c['namespace'],ck,cmname)#class_str='%s\t\t.def_static("%s", %s(&%s::%s::%s)%s)\n' % (class_str,cmname,method_cast, c['namespace'],ck,cmname,param_str)
                        class_str_cppfile='%s\t\t.def_static("%s", static_cast< %s (*)(%s) >(&%s::%s::%s))\n' % (class_str_cppfile,cmname,cm['rtnType'],temp_param_str, c['namespace'],ck,cmname)#class_str_cppfile='%s\t\t.def_static("%s", %s(&%s::%s::%s)%s)\n' % (class_str_cppfile,cmname,method_cast, c['namespace'],ck,cmname,param_str)
                    elif 'py::args' in linestr_api or 'py::kwargs' in linestr_api:
                        class_str='%s\t\t.def_static("%s", &%s::%s::%s)\n' % (class_str,cmname,c['namespace'],ck,cmname )
                        class_str_cppfile='%s\t\t.def_static("%s", &%s::%s::%s)\n' % (class_str_cppfile,cmname,c['namespace'],ck,cmname )                        
                    else:
                        class_str='%s\t\t.def_static("%s", &%s::%s::%s%s)\n' % (class_str,cmname,c['namespace'],ck,cmname,param_str)
                        class_str_cppfile='%s\t\t.def_static("%s", &%s::%s::%s%s)\n' % (class_str_cppfile,cmname,c['namespace'],ck,cmname,param_str)
                else:
                    if cm_count.get(cmname,0) > 1: # cmname.startswith("set_") and nParams==1 and "string" in method_cast:
                        class_str='%s\t\t.def("%s", %s(&%s::%s::%s)%s)\n' % (class_str,cmname,method_cast,c['namespace'], ck,cmname,param_str)
                        class_str_cppfile='%s\t\t.def("%s", %s(&%s::%s::%s)%s)\n' % (class_str_cppfile,cmname,method_cast,c['namespace'], ck,cmname,param_str)
                    elif 'py::args' in linestr_api or 'py::kwargs' in linestr_api:
                        class_str='%s\t\t.def("%s", &%s::%s::%s)\n' % (class_str,cmname,c['namespace'],ck,cmname )
                        class_str_cppfile='%s\t\t.def("%s", &%s::%s::%s)\n' % (class_str_cppfile,cmname,c['namespace'],ck,cmname )                            
                    else:
                        class_str='%s\t\t.def("%s", &%s::%s::%s,%s)\n' % (class_str,cmname,c['namespace'],ck,cmname,param_str)
                        class_str_cppfile='%s\t\t.def("%s", &%s::%s::%s%s)\n' % (class_str_cppfile,cmname,c['namespace'],ck,cmname,param_str)                      
        class_str='%s\t\t;\n\n' % (class_str)
        class_str_cppfile='%s\t\t;\n\n' % (class_str_cppfile)
        class_str_cppfile='%s}\n\n' % (class_str_cppfile)
    ##for ck
    dd['include_str']=include_str
    dd['enum_str']=enum_str
    dd['class_str']=class_str
    dd['include_str_hfile']=include_str_hfile
    dd['enum_str_hfile']=enum_str_hfile
    dd['enum_str_cppfile']=enum_str_cppfile
    dd['class_str_hfile']=class_str_hfile
    dd['class_str_cppfile']=class_str_cppfile
    dd['init_module_str']=init_module_str
    dd['init_module_include_str']='#include "pybind_%s.h" \n' % (stem_filename)
#   
    output_hfile='%s/pybind_%s.h' % (outputdir,stem_filename)
    output_cppfile='%s/pybind_%s.cpp' % (outputdir,stem_filename)
    with open(output_hfile,'w') as hfile:
        hfile.write(include_str_hfile)
        hfile.write(enum_str_hfile)
        hfile.write(class_str_hfile)
        hfile.write('\n\n#endif')
    with open(output_cppfile,'w') as cppfile:
        cppfile.write('#include "pybind_%s.h" \n' % (stem_filename) )
        cppfile.write(enum_str_cppfile)
        cppfile.write(class_str_cppfile)
    return dd
######    
def get_pybind_for_dir_cfginfo_map(dir_cfginfo_map):
    if dir_cfginfo_map is None or len(dir_cfginfo_map)==0:
        return {}
    result={}
    result['file_str_list']=[]
    include_str=''
    enum_class_str=''
    outputdir='.' #dir_cfginfo_map.get("outputdir",".")
    common_include_str=''
    init_module_str='\tinit_common(m);\n'
    init_module_include_str=''
    for (dir_name,dir_cfginfo) in dir_cfginfo_map.items():
        common_include_str=dir_cfginfo.get('include_str',common_include_str)
        outputdir=dir_cfginfo.get('outputdir',outputdir)
        file_cfginfos=get_file_cfginfos_for_dir_cfginfo(dir_cfginfo)
        if file_cfginfos is None or len(file_cfginfos)==0:
            continue
        for file_cfginfo in file_cfginfos:
            dd=get_pybind_for_file_cfginfo(file_cfginfo)
            if dd is None or len(dd)==0:
                continue
            filename=file_cfginfo['filename']
            result['file_str_list'].append(dd)
#            include_str='%s\n//begin_filename:%s' % (include_str,filename)
            enum_class_str='%s\n//begin_filename:%s' % (enum_class_str,filename)
            init_module_str='%s\n//file:%s' % (init_module_str,filename)
            init_module_str='%s\n%s' % (init_module_str,dd.get('init_module_str',''))
            init_module_include_str='%s\n%s' % (init_module_include_str,dd.get('init_module_include_str',''))
            if 'include_str' in dd:
                include_str='%s\n%s' %(include_str, dd['include_str'])
            if 'enum_str' in dd:
                enum_class_str='%s\n%s' %(enum_class_str,dd['enum_str'])
            if 'class_str' in dd:
                enum_class_str='%s\n%s' % (enum_class_str,dd['class_str'])
#            include_str='%s\n//end_filename:%s\n\n' % (include_str,filename)
            enum_class_str='%s//end_filename:%s\n\n' % (enum_class_str,filename)                
    result['include_str']=include_str
    result['enum_class_str']=enum_class_str
    tempfile="./temp_pybind.cpp" #"%s/temp_pybind.cpp" % (outputdir)
    tempf=open(tempfile,'w')
    tempf.write(include_str)
    tempf.write('\n\n')
    tempf.write(enum_class_str)
    tempf.close()  
    initfilename="%s/init_module_pybind.cpp" % (outputdir)
    with open(initfilename,"w") as initfile:
        initfile.write(common_include_str)
        initfile.write('#include "init_module_common.h"\n')
        initfile.write("\n//////\n")
        initfile.write(init_module_include_str)
        initfile.write("////////////\n")
        initfile.write('#ifdef PYTHON_VERSION_2\n')
        initfile.write('PYBIND11_MODULE(pytiledb2, m)\n')
        initfile.write('#else\n')
        initfile.write('PYBIND11_MODULE(pytiledb, m)\n')
        initfile.write('#endif\n')
        initfile.write('{\n') 
        initfile.write(init_module_str)
        initfile.write('\n}//PYBIND11_MODULE(pytiledb, m)\n')         
    return result        
######
def get_pybind_for_tiledb():
    dir_cfginfo_map=get_tiledb_dir_cfginfo_map()
    return get_pybind_for_dir_cfginfo_map(dir_cfginfo_map)
def get_tiledb_dir_cfginfo_map():
    default_dir_cfginfo=get_default_cfginfo()
    default_dir_cfginfo['outputdir']='../cpp/src/tiledb_pybind'
    default_dir_cfginfo['namespaces']=['tiledb','IB']
#    
    cppapi_dir_cfginfo=copy.deepcopy(default_dir_cfginfo)
    cppapi_dir_cfginfo["dir"]="../cpp/src/tiledb/cxx_api"
    cppapi_dir_cfginfo["ignore_file_keywords"]=['typedef','deleter','schema_base.h','arrow_io',"tiledb.h","tiledb_cxx.h","tiledb_cxx_type.h","tiledb_cxx_core_interface","tiledb_cxx_object","tiledb_cxx_utils","tiledb_enum","tiledb_export","tiledb_serialization","tiledb_struct_def","tiledb_version"]

    cppapi_dir_cfginfo['ignore_class_keywords']=['VectorData','MapData','PrimitiveColumnData','VectorColumnData','GenericColumnData']
#    
    result={"cxxapi":cppapi_dir_cfginfo}
    return result    
    
######
###get tiledb 
def get_swig_for_file_cfginfo(file_cfginfo):
    dd={}
##    base_cfginfo=get_base_cfginfo()
    if file_cfginfo is None:
        return dd
    if not "filename" in file_cfginfo:
        return dd
#    
    ignore_ptr_keywords=file_cfginfo.get('ignore_ptr_keywords',[])
    ptr_keywords=file_cfginfo.get('ptr_keywords',[])
    ignore_class_keywords=file_cfginfo.get('ignore_class_keywords',[])
    ignore_method_keywords=file_cfginfo.get('ignore_method_keywords',[])
#  
    outputdir=file_cfginfo.get('outputdir','./')
    filename=file_cfginfo['filename']
    tempitems=filename.split('/')
    full_filename=tempitems[-1]
    tempitems=tempitems[-1].split('.')
    tempitem=tempitems[0]
    stem_filename=tempitem
    protobuf_inttypes=["google::protobuf::int32","google::protobuf::uint32","google::protobuf::int64","google::protobuf::uint64"]
    ns=['tiledb']
    if 'namespaces' in file_cfginfo:
        ns=file_cfginfo['namespaces']
    ns0=''
    if len(ns)>0:
        ns0=ns[0]
    shared_ptr_str=""
    include_copy_block='#include "%s.h"\n' % (stem_filename)
    ignore_str="//ignore class or methods in file:%s\n" % (filename)    
    include_headers='%%include "%s.h"\n' % (stem_filename)
    cppheader=get_chp(filename)  #parse head file
    for ck in cppheader.classes.keys():
        c=cppheader.classes[ck]
        if (not c['namespace'] in ns) or is_any_of_words_in_str(ignore_class_keywords,ck):
            linfo='ignore class namespace %s::%s'  % (c['namespace'],c['name'])
            ignore_str='%s%%ignore %s;\n' % (ignore_str,c['name'])
            print(linfo)
            continue
        linfo='start to process class %s::%s'  % (c['namespace'],c['name'])
        if is_any_of_words_in_str(ignore_ptr_keywords,ck) or ck.endswith("Util") or ck.endswith("Type") or ck.endswith("Mgr") or ck.endswith("Mask") or ck.endswith("Status") or ck.endswith("TypeHelper"):
            linfo='not shared_ptr for %s' % (c['name'])
        else:
            shared_ptr_str='%s%%shared_ptr(%s::%s)\n' % (shared_ptr_str,c['namespace'],c['name'])    
        for cm in c['methods']['public']:
            cmname=cm['name']
            is_ignore_method=False
            linestr_api=''
            linestr_pb=''
            linestr_call='('
            namespace_prefix='%s::' % (c['namespace'])
            return_type=cm['rtnType']
            if "static" in return_type:
                method_cast='static_cast<%s (*)(' % (return_type.replace("static",""))
            else:
                method_cast='(%s (%s::%s::*)(' % (return_type,c['namespace'],ck)
#            return_type=CommonUtil.add_prefix_for_words_in_str(namespace_data_type_keywords,return_type,namespace_prefix)
            if cm['virtual']:
                linestr_api=linestr_api+"virtual "
                linestr_pb=linestr_pb+"virtual "   
            linestr_api=linestr_api+ return_type #cm['rtnType']
            linestr_api='\t%s %s(' % (linestr_api,cm['name'])
            linestr_pb=linestr_pb + return_type #cm['rtnType']
            linestr_pb='\t%s %s(' % (linestr_pb, cm['name'])   
            nParams=len(cm['parameters'])
            pcount=0
            for p in cm['parameters']:
                if pcount>0:
                    linestr_api="%s," % (linestr_api)
                    linestr_pb="%s," % (linestr_pb)
                    linestr_call='%s,' % (linestr_call)
                    method_cast='%s,' % (method_cast)
                param_type=p['type']
#                param_type=CommonUtil.add_prefix_for_words_in_str(namespace_data_type_keywords,param_type,namespace_prefix)
                linestr_api="%s %s %s" % (linestr_api,param_type,p['name'])
                linestr_call='%s%s' % (linestr_call,p['name'])
                method_cast='%s%s' % (method_cast,p['type'])
                pcount=pcount+1  
######                        
            nParams=len(cm['parameters'])
            pcount=0
            param_str=''
            orig_param_str=''
            for p in cm['parameters']:
                if pcount==0:
                    orig_param_str=p['type']
                else:
                    orig_param_str='%s,%s' % (orig_param_str,p['type'])
                param_str='%s, py::arg("%s")' % (param_str,p['name'])    
                pcount=pcount+1                
            linestr_api='%s)' % (linestr_api)
            linestr_api=linestr_api.replace("const::google::protobuf::Map", "const ::google::protobuf::Map")
            linestr_pb='%s)' % (linestr_pb)
            linestr_call='%s)' % (linestr_call) 
            if cm['const']:
                if 'static' in return_type or ('static' in cm.keys() and cm['static']):
                    method_cast='%s) const >' % (method_cast)
                else:
                    method_cast='%s) const)' % (method_cast)
            else:
                if 'static' in return_type or ('static' in cm.keys() and cm['static']):
                    method_cast='%s)>' % (method_cast)
                else:
                    method_cast='%s))' % (method_cast)
#
            if cmname.startswith('mutable_') or cmname.startswith('release_') or cmname.startswith('set_allocated_'):
                is_ignore_method=True  
            elif cmname.startswith('set_') and len(cm['parameters'])==1 and (cm['parameters'][0]['type']=='const char *' or cm['parameters'][0]['type']=='char const *') :
                is_ignore_method=True
            elif cmname.startswith('set_') and len(cm['parameters'])==2 and (cm['parameters'][0]['type']=='const char *' or cm['parameters'][0]['type']=='char const *') and cm['parameters'][1]['type']=='size_t':
                is_ignore_method=True
            elif is_any_of_words_in_str(ignore_method_keywords, linestr_api): ## ('operator' in linestr_api): # and cm['constructor']:
                is_ignore_method=True
###            elif c['namespace']=='tiledb' and   'Type' in linestr_api:
###                is_ignore_method=True     
            if is_ignore_method:
                ignore_str='%s%%ignore %s::%s::%s(%s);\n' % (ignore_str,c['namespace'],c['name'],cmname,orig_param_str)
    dd['shared_ptr_str']=shared_ptr_str
    dd['include_copy_block']=include_copy_block
    dd['ignore_str']=ignore_str
    dd['include_headers']=include_headers
    return dd

def get_swig_for_tiledb(outputfile='swig_tiledb.i'):
    dd={}
    shared_ptr_str=''
    include_copy_block=''
    ignore_str=''
    include_headers=''
    dir_cfginfo_map=get_tiledb_dir_cfginfo_map()
    for (dir_name,dir_cfginfo) in dir_cfginfo_map.items():
        file_cfginfos=get_file_cfginfos_for_dir_cfginfo(dir_cfginfo)
        if file_cfginfos is None or len(file_cfginfos)==0:
            continue
        for file_cfginfo in file_cfginfos: 
            dd_file=get_swig_for_file_cfginfo(file_cfginfo)
            shared_ptr_str='%s\n%s' % (shared_ptr_str,dd_file['shared_ptr_str'])
            include_copy_block='%s\n%s' % (include_copy_block,dd_file['include_copy_block']) 
            ignore_str='%s\n%s' % (ignore_str,dd_file['ignore_str'])
            include_headers='%s\n%s' % (include_headers,dd_file['include_headers']) 
    with open(outputfile,"w") as swigfile:
        swigfile.write('#ifndef SWIG_TILEDB_I\n')
        swigfile.write('#define SWIG_TILEDB_I\n')
        swigfile.write('\n//////include other i files\n')
        swigfile.write('%include swig_common.i\n')
        swigfile.write('//%include protobuf.i\n')
        swigfile.write('\n//////\n')
        swigfile.write('\n//////shared_ptr\n')
        swigfile.write(shared_ptr_str)
        swigfile.write('\n//////end shared_ptr\n')
        swigfile.write('\n//////\n')
        swigfile.write('%{\n')
        swigfile.write(include_copy_block)
        swigfile.write('%}\n')
        swigfile.write('\n//////ignore\n')
        swigfile.write(ignore_str)
        swigfile.write('\n//////end ignore\n')
        swigfile.write('\n//////headers\n')
        swigfile.write(include_headers)
        swigfile.write('\n//////end headers\n')
        swigfile.write('\n#endif\n')  
    dd['shared_ptr_str']=shared_ptr_str
    dd['include_copy_block']=include_copy_block 
    dd['ignore_str']=ignore_str 
    dd['include_headers']=include_headers
    return dd 